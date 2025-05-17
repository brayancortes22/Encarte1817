using Business.Interfaces;
using Entity.Dtos.UserDTO;
using Entity.Model;

namespace Business.Interfaces
{
    ///<summary>
    /// Define los m�todos de negocio espec�ficos para la gest�on de usuarios.
    ///Hereda operaciones CRUD gen�ricas de <see cref="IBaseBusiness{User, UserDto}"/>.
    ///</summary>
    public interface IUserBusiness : IBaseBusiness<User, UserDto>
    {
        /// <summary>
        /// Obtiene un usuario por su direcci�n de correo electr�nico.
        /// </summary>
        /// <param name="email">Correo electr�nico del usuario a buscar.</param>
        /// <returns>
        /// El <see cref="User"/> que coincida con el correo proporcionado o <c>null</c> si no se encuentra.
        /// </returns>
        Task<User> GetByEmailAsync(string email);

        /// <summary>
        /// Valida las credenciales de un usuario comparando el correo y la contrase�a.
        /// </summary>
        /// <param name="email">Correo electr�nico del usuario.</param>
        /// <param name="password">Contrase�a en texto plano para validar.</param>
        ///<returns>True si las credenciales son v�lidas; de lo contario false</returns>
        Task<bool> ValidateCredentialsAsync(string email, string password);

        /// <summary>
        /// Actualiza parcialmente los datos de un usuario.
        /// </summary>
        /// <param name="dto">Objeto que contiene los datos parciales a actualizar del usuario.</param>
        ///<returns>True si la actualizaci�n fue exitosa; de lo contario false</returns>
        Task<bool> UpdateParcialUserAsync(UpdateUserDto dto);

        /// <summary>
        /// Cambia el estado activo/inactivo de un usuario.
        /// </summary>
        /// <param name="dto">Objeto que contiene el ID del usuario y el nuevo estado activo.</param>
        ///<returns>True si el cambio de estado fue exitoso; de lo contario false</returns>

        Task<bool> SetUserActiveAsync(DeleteLogicalUserDto dto);
    }
}
