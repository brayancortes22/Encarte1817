// Business/Services/JwtService.cs
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Interfaces;
using Entity.Dtos.AuthDTO;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;
        private readonly IUserBusiness _userBusiness;
        private readonly IRolBusiness _rolBusiness;
        private readonly IRoleUserBusiness _roleUserBusiness;
        private readonly IRefreshTokenData _refreshTokenData;

        public JwtService(
            IConfiguration configuration,
            ILogger<JwtService> logger,
            IUserBusiness userBusiness,
            IRolBusiness rolBusiness,
            IRoleUserBusiness roleUserBusiness,
            IRefreshTokenData refreshTokenData)
        {
            _configuration = configuration;
            _logger = logger;
            _userBusiness = userBusiness;
            _rolBusiness = rolBusiness;
            _roleUserBusiness = roleUserBusiness;
            _refreshTokenData = refreshTokenData;
        }

        public async Task<TokenResponseDto> GenerateTokenAsync(User user, IEnumerable<string> roles)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
                
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                   // new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                };
                
                // Agregar roles como claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationMinutes"])),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                
                // Generar refresh token y guardar en BD
                var refreshToken = await SaveRefreshTokenAsync(user.Id);

                return new TokenResponseDto
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken,
                    ExpiresIn = Convert.ToInt32(_configuration["Jwt:ExpirationMinutes"]) * 60,
                    TokenType = "Bearer"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al generar token: {ex.Message}");
                throw;
            }
        }

        private async Task<string> SaveRefreshTokenAsync(int userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateRefreshTokenString(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpirationDays"])),
                CreatedAt = DateTime.UtcNow,
                IsUsed = false,
                IsRevoked = false
            };

            await _refreshTokenData.CreateAsync(refreshToken);
            return refreshToken.Token;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
                
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al validar token: {ex.Message}");
                throw;
            }
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                // Buscar el refresh token en BD
                var storedToken = await _refreshTokenData.GetByTokenAsync(refreshToken);
                
                // Validar refresh token
                if (storedToken == null)
                {
                    throw new SecurityTokenException("Refresh token no encontrado");
                }
                
                if (storedToken.IsUsed)
                {
                    throw new SecurityTokenException("El refresh token ya ha sido utilizado");
                }
                
                if (storedToken.IsRevoked)
                {
                    throw new SecurityTokenException("El refresh token ha sido revocado");
                }
                
                if (storedToken.ExpiryDate < DateTime.UtcNow)
                {
                    throw new SecurityTokenException("El refresh token ha expirado");
                }

                // Obtener el usuario asociado al refresh token
                var user = await _userBusiness.GetByIdAsync(storedToken.UserId);
                if (user == null || !user.Active)
                {
                    throw new SecurityTokenException("Usuario no encontrado o inactivo");
                }

                // Marcar token como usado
                await _refreshTokenData.MarkTokenAsUsedAsync(refreshToken);

                // Obtener roles del usuario
                var userRoles = await _roleUserBusiness.GetAllAsync();
                var userRoleIds = userRoles
                    .Where(ur => ur.UserId == user.Id)
                    .Select(ur => ur.RolId)
                    .ToList();

                var roles = (await _rolBusiness.GetAllAsync())
                    .Where(r => userRoleIds.Contains(r.Id))
                    .Select(r => r.Name)
                    .ToList();

                // Generar nuevo token
                return await GenerateTokenAsync(user, roles);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al refrescar token: {ex.Message}");
                throw;
            }
        }

        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}