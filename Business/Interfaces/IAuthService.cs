// Business/Interfaces/IAuthService.cs
using System.Threading.Tasks;
using Entity.Dtos.AuthDTO;

namespace Business.Interfaces
{
    /// <summary>
    /// Define los métodos y operaciones de autenticación dispobibles en el sistemas
    ///</summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica a un usuario utilizando las credenciales proporcionadas y genera un token JWT.
        /// </summary>
        /// <param name="request">Objeto que contiene el correo electrónico y la contraseña del usuario.</param>
        /// <returns> Un objeto con el token JWT, el refresh token y datos asociados al usuario.</returns>
        Task<TokenResponseDto> LoginAsync(LoginRequestDto request);

        /// <summary>
        /// Renueva el token de acceso utilizando un refresh token válido.
        /// </summary>
        /// <param name="request">Objeto que contiene el refresh token actual y el ID del usuario.</param>
        /// <returns>Un nuevo token de acceso y un nuevo refresh token si todo es correcto.</returns>
        Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        /// <summary>
        /// Cierra la sesión del usuario, invalidando el refresh token activo.
        /// </summary>
        /// <param name="userId">ID del usuario que desea cerrar sesión.</param>
        ///<returns>True si el cierre de sesión fue exitoso; de lo contario false</returns>
        Task<bool> LogoutAsync(int userId);
    }
}


