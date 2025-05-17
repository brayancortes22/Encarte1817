// Business/Services/AuthService.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.AuthDTO;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IJwtService _jwtService;
        private readonly IRoleUserBusiness _roleUserBusiness;
        private readonly IRolBusiness _rolBusiness;
        private readonly IRefreshTokenData _refreshTokenData;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserBusiness userBusiness,
            IJwtService jwtService,
            IRoleUserBusiness roleUserBusiness,
            IRolBusiness rolBusiness,
            IRefreshTokenData refreshTokenData,
            ILogger<AuthService> logger)
        {
            _userBusiness = userBusiness;
            _jwtService = jwtService;
            _roleUserBusiness = roleUserBusiness;
            _rolBusiness = rolBusiness;
            _refreshTokenData = refreshTokenData;
            _logger = logger;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginRequestDto request)
        {
            try
            {
                // Validar credenciales
                var isValid = await _userBusiness.ValidateCredentialsAsync(request.Email, request.Password);
                if (!isValid)
                {
                    throw new UnauthorizedAccessException("Credenciales inválidas");
                }

                // Obtener el usuario
                var user = await _userBusiness.GetByEmailAsync(request.Email);
                if (user == null || !user.Active)
                {
                    throw new UnauthorizedAccessException("Usuario no encontrado o inactivo");
                }

                // Revocar todos los refresh tokens anteriores del usuario
                await _refreshTokenData.RevokeAllUserTokensAsync(user.Id);

                // Obtener roles del usuario
                var userRoles = await _roleUserBusiness.GetAllAsync();
                var userRoleIds = userRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => ur.RolId)
                    .ToList();

                // Obtener nombres de roles
                var roles = (await _rolBusiness.GetAllAsync())
                    .Where(r => userRoleIds.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToList();

                // Generar token
                return await _jwtService.GenerateTokenAsync(user, roles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en login: {ex.Message}");
                throw;
            }
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            try
            {
                return await _jwtService.RefreshTokenAsync(request.RefreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al refrescar token: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                // Revocar todos los refresh tokens del usuario
                return await _refreshTokenData.RevokeAllUserTokensAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en logout: {ex.Message}");
                throw;
            }
        }
    }
}