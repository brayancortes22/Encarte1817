using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.UserDTO;
using Entity.Model;
using Web.Controllers.Interface;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    [Route("api/[controller]")]
    public class UserController : GenericController<UserDto, User>, IUserController
    {
        private readonly IUserBusiness _userBusiness;

        public UserController(IUserBusiness userBusiness, ILogger<UserController> logger)
            : base(userBusiness, logger)
        {
            _userBusiness = userBusiness;
        }

        protected override int GetEntityId(UserDto dto)
        {
            return dto.Id;
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userBusiness.GetByEmailAsync(email);
                if (user == null)
                    return NotFound($"Usuario con email {email} no encontrado");

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al obtener usuario con email {email}: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePartialUser([FromBody] UpdateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _userBusiness.UpdateParcialUserAsync(dto);
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error de validación al actualizar parcialmente usuario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar parcialmente usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }



        //Este metodo responde a patch /users//{id}/status
        [HttpPatch("users/{id}/status")]
        public async Task<IActionResult> SetUserActive([FromBody] UserStatusDto dto)
        {
            try
            {
                //validacion de modelo

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                    //llamamos a la logica de negocio pasando el ID y el nuevo estado

                var result = await _userBusiness.SetUserActiveAsync(id,dto.IsActive);
                //Responde con 200 OK el resultado
                return Ok(new { Success = result });
            }
            catch (ArgumentException ex)
            {
                //Captura errores de validacion  por ejemplo, usuario no encontrado
                _logger.LogError($"Error de validación al cambiar estado de usuario: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //Captura errores inesperados
                _logger.LogError($"Error al cambiar estado de usuario: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateCredentials([FromBody] LoginRequestDto loginDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
                    return BadRequest("Email y contraseña son requeridos");

                var isValid = await _userBusiness.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);
                if (!isValid)
                    return Unauthorized("Credenciales inválidas");

                return Ok(new { IsValid = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al validar credenciales: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }


//prueba 
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}