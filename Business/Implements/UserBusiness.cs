using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Business.Interfaces;
using Business.Services;
using Data.Implements.RolUserData;
using Data.Interfaces;
using Entity.Dtos.RolUserDTO;
using Entity.Dtos.UserDTO;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;
using Utilities.Interfaces;
using Utilities.Mail;
using ValidationException = Utilities.Exceptions.ValidationException;

namespace Business.Implements
{
    /// <summary>
    /// Contiene la logica de negocio de los metodos especificos para la entidad Rol
    /// Extiende BaseBusiness heredando la logica de negocio de los metodos base 
    /// </summary>
    public class UserBusiness : BaseBusiness<UserDto, User>, IUserBusiness
    {
        private readonly IUserData _userData;
        private readonly IEmailService _emailService;

        public UserBusiness(IUserData userData, IMapper mapper, ILogger<UserBusiness> logger, IGenericIHelpers helpers, IEmailService emailService)
            : base(userData, logger, mapper, helpers)
        {
            _userData = userData;
            _emailService = emailService;
        }

        ///<summary>
        /// Obtiene un usuario por su dirección de correo electrónico.
        ///</summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <returns>El objeto User si existe; de lo contrario, null.</returns>
        public async Task<User> GetByEmailAsync(string email)
        {
            var users = await _userData.GetAllAsync();
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        ///<summary>
        /// Verifica las credenciales del usuario para iniciar sesión
        ///</summary>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="password">Contraseña en texto plano.</param>
        /// <returns>El objeto User autenticado si las credenciales son válidas; de lo contrario, null.</returns>
        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user == null || !user.Status)
                return null;

            string hashedPassword = HashPassword(password);
            return user.Password == hashedPassword ? user : null;
        }

        /// <summary>
        /// Genera un hash SHA-256 a partir de una contraseña en texto plano.
        /// </summary>
        /// <param name="password">Contraseña en texto plano a ser hasheada.</param>
        /// <returns>Una cadena en formato hexadecimal que representa el hash SHA-256 de la contraseña.</returns>
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        ///<summary>
        /// Crea un nuevo usuario con su contraseña hasheada y estado activo.
        ///</summary>
        /// <param name="dto">Objeto UserDto con los datos del nuevo usuario.</param>
        /// <returns>El usuario creado como UserDto.</returns>
        public async Task<UserDto> CreateAsync(UserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            user.Password = HashPassword(user.Password);
            user.Status = true;
            user.CreatedAt = DateTime.Now;

            var createdUser = await _userData.CreateAsync(user);
            return _mapper.Map<UserDto>(createdUser);

        }

        /// <summary>
        /// Actualiza parcialmente los datos de un usuario.
        /// </summary>
        /// <param name="dto">Objeto UpdateUserDto con los datos a modificar.</param>
        /// <returns> True si la actualización fue exitosa; de lo contrario, false.</returns>
        /// <exception cref="ArgumentException"> Se lanza si el ID del usuario es inválido.</exception>
        public async Task<bool> UpdateParcialUserAsync(UpdateUserDto dto)
        {
            if (dto.Id <= 0)
                throw new ArgumentException("ID inválido.");

            var user = _mapper.Map<User>(dto);

            var result = await _userData.UpdatePartial(user); // esto ya retorna bool
            return result;
        }

        /// <summary>
        /// Activa o desactiva un usuario de forma lógica según su ID.
        /// </summary>
        /// <param name="dto">Objeto DeleteLogicalUserDto" con el ID y estado deseado.</param>
        /// <returns>True si se actualizó el estado correctamente; de lo contrario, false.</returns>
        /// <exception cref="ValidationException">Si el ID es inválido.</exception>
        /// <exception cref="EntityNotFoundException">Si el usuario no existe.</exception>
        public async Task<bool> SetUserActiveAsync(DeleteLogicalUserDto dto)
        {
            if (dto == null || dto.Id <= 0)
                throw new ValidationException("Id", "El ID del usuario es inválido");

            var exists = await _userData.GetByIdAsync(dto.Id)
                ?? throw new EntityNotFoundException("user", dto.Id);

            return await _userData.Active(dto.Id, dto.Status);
        }

        /// <summary>
        /// Cambia la contraseña de un usuario identificado por su ID.
        /// </summary>
        /// <param name="dto">Objeto <see cref="UpdatePasswordUserDto"/> con ID y nueva contraseña.</param>
        /// <returns>True si la contraseña se cambió exitosamente; de lo contrario,false.</returns>
        public async Task<bool> ChangePasswordAsync(UpdatePasswordUserDto dto)
        {
            // Aquí puedes hacer validaciones si es necesario

            // Llamada al Data con los datos extraídos del DTO
            return await _userData.ChangePasswordAsync(dto.Id, dto.Password);
        }

        /// <summary>
        /// Asigna un rol a un usuario.
        /// </summary>
        /// <param name="dto">Objeto <see cref="AssignUserRolDto"/> con ID de usuario e ID de rol.</param>
        /// <returns>True si se asignó el rol correctamente; de lo contrario, false.</returns>
        public async Task<bool> AssignRolAsync(AssignUserRolDto dto)
        {
            // Aquí puedes hacer validaciones si deseas
            return await _userData.AssingRolAsync(dto.UserId, dto.RolId);
        }


        /// <summary>
        /// Notifica al usuario mediante correo electrónico sobre la creación de su cuenta.
        /// </summary>
        /// <param name="emailDestino">Dirección de correo electrónico del destinatario.</param>
        /// <param name="nombre">Nombre del usuario para personalizar el mensaje.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        /// <exception cref="Exception">Se lanza cuando el envío del correo falla.</exception>
        public async Task NotificarUsuarioAsync(string emailDestino, string nombre)
        {

            string asunto = "Bienvenido al sistema";
            string cuerpo = $"Hola {nombre}, tu cuenta ha sido creada con éxito.";

            _logger.LogInformation($"Enviando correo de notificación a {emailDestino}");

            bool enviado = await _emailService.SendEmailAsync(emailDestino, asunto, cuerpo);
            if (!enviado)
            {
                _logger.LogError($"Error al enviar correo de notificación a {emailDestino}");
                throw new Exception("No se pudo enviar el correo de notificación.");
            }

            _logger.LogInformation($"Correo de notificación enviado exitosamente a {emailDestino}");
        }
    }
}