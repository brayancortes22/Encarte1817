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
    /// Define los m�todos relacionados con la generaci�n, validaci�n y renovaci�n de Tokens JWT.
    ///</summary>
    public interface IJwtService
    {
        /// <summary>
        /// Genera un token JWT y un refresh token para un usuario autenticado.
        /// </summary>
        /// <param name="user">El usuario autenticado para el cual se generar� el token.</param>
        /// <param name="roles">Una lista de roles asociados al usuario.</param>
        /// <returns>
        /// Un <see cref="TokenResponseDto"/> que contiene el token JWT, el refresh token, y otros datos del usuario.
        /// </returns>
        Task<TokenResponseDto> GenerateTokenAsync(User user, IEnumerable<string> roles);

        /// <summary>
        /// Valida un token JWT y extrae los claims asociados.
        /// </summary>
        /// <param name="token">El token JWT a validar.</param>
        /// <returns>
        /// Un <see cref="ClaimsPrincipal"/> que contiene los claims si el token es v�lido; de lo contrario, <c>null</c>.
        /// </returns>
        ClaimsPrincipal ValidateToken(string token);

        /// <summary>
        /// Genera un nuevo token JWT utilizando un refresh token v�lido.
        /// </summary>
        /// <param name="refreshToken">El refresh token v�lido asociado a un usuario.</param>
        /// <returns>
        /// Un <see cref="TokenResponseDto"/> con un nuevo token de acceso y un nuevo refresh token.
        /// </returns>
        Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    }
}