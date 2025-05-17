// Business/Interfaces/IJwtService.cs
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.Dtos.AuthDTO;
using Entity.Model;

namespace Business.Interfaces
{
    ///<summary>
    /// Define los métodos relacionados con la generación, validación y renovación de Tokens JWT.
    ///</summary>
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT y un refresh token para un usuario autenticado.
        /// </summary>
        /// <param name="user">El usuario autenticado para el cual se generará el token.</param>
        /// <param name="roles">Una lista de roles asociados al usuario.</param>
        /// <returns>Objeto que incluye el token de acceso, refresh token y datos útiles para el cliente.</returns>
        Task<TokenResponseDto> GenerateTokenAsync(User user, IEnumerable<string> roles);

        /// <summary>
        /// Valida un token JWT y extrae los claims asociados.
        /// </summary>
        /// <param name="token">El token JWT a validar.</param>
        /// <returns>Claims extraídos del token si es válido; si no, devuelve null o lanza una excepción.</returns>
        ClaimsPrincipal ValidateToken(string token);

        /// <summary>
        /// Genera un nuevo token JWT utilizando un refresh token válido.
        /// </summary>
        /// <param name="refreshToken">El refresh token válido asociado a un usuario.</param>
        /// <returns>Nuevo token JWT y refresh token actualizado.</returns>
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    }
}