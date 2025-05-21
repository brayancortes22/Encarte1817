using Microsoft.AspNetCore.Mvc;
using Entity.Dtos.CredencialesDTO;
using System.Collections.Generic;
using System.Linq;
using Entity.Dtos.AuthDTO;
using Business.Interfaces;

namespace Web.Controllers.Implements
{
    // Indica que este controlador responde a peticiones HTTP tipo API
    [ApiController]

    // Define la ruta base para acceder a este controlador
    // Por defecto, ser�: api/credenciales
    [Route("api/[controller]")]
    public class CredencialesController : ControllerBase
    {
        // Simulaci�n de una base de datos en memoria (solo para pruebas)
        // Es una lista est�tica para guardar las credenciales
        private static List<CredencialesDto> _credenciales = new List<CredencialesDto>();

        // Inyecci�n del logger para registrar informaci�n, errores, etc.
        private readonly IAuthService _authService;
        private readonly ILogger<CredencialesController> _logger;

        // Constructor del controlador, recibe el logger autom�ticamente
        public CredencialesController(IAuthService authService, ILogger<CredencialesController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // Nuevo endpoint para login con JWT
        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login([FromBody] CredencialesDto credenciales)
        {
            try
            {
                // Validaci?n de modelo
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Obtenemos el token usando el servicio de autenticaci?n
                var authResult = await _authService.LoginAsync(credenciales);

                // Devolvemos el resultado con el token JWT
                return Ok(authResult);
            }
            catch (UnauthorizedAccessException)
            {
                _logger.LogWarning($"Intento de login fallido para {credenciales.Email}");
                return Unauthorized("Credenciales inv?lidas");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error durante el login: {ex.Message}");
                return StatusCode(500, "Error interno del servidor");
            }
        }


        // GET: api/credenciales
        // Devuelve la lista completa de credenciales
        [HttpGet]
        public ActionResult<IEnumerable<CredencialesDto>> GetAll()
        {
            return Ok(_credenciales);
        }

        // GET: api/credenciales/5
        // Busca y devuelve una credencial por ID
        [HttpGet("{id}")]
        public ActionResult<CredencialesDto> GetById(int id)
        {
            // Busca el elemento en la lista
            var credencial = _credenciales.FirstOrDefault(c => c.Id == id);

            // Si no se encuentra, devuelve 404 Not Found
            if (credencial == null)
                return NotFound();

            // Si se encuentra, lo devuelve con estado 200 OK
            return Ok(credencial);
        }

        // POST: api/credenciales
        // Crea una nueva credencial
        [HttpPost]
        public ActionResult<CredencialesDto> Create(CredencialesDto dto)
        {
            // Genera un nuevo ID autom�tico
            dto.Id = _credenciales.Count > 0 ? _credenciales.Max(c => c.Id) + 1 : 1;

            // Agrega el nuevo elemento a la lista
            _credenciales.Add(dto);

            // Devuelve 201 Created con la ubicaci�n del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/credenciales/5
        // Actualiza una credencial existente
        [HttpPut("{id}")]
        public IActionResult Update(int id, CredencialesDto updated)
        {
            // Busca la credencial en la lista
            var existing = _credenciales.FirstOrDefault(c => c.Id == id);

            // Si no se encuentra, devuelve 404
            if (existing == null)
                return NotFound();

            // Actualiza los datos
            existing.Email = updated.Email;
            existing.Password = updated.Password;

            // Devuelve 204 No Content para indicar �xito sin contenido adicional
            return NoContent();
        }

        // DELETE: api/credenciales/5
        // Elimina una credencial por ID
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            // Busca el elemento
            var existing = _credenciales.FirstOrDefault(c => c.Id == id);

            // Si no se encuentra, devuelve 404
            if (existing == null)
                return NotFound();

            // Elimina el elemento de la lista
            _credenciales.Remove(existing);

            // Devuelve 204 No Content
            return NoContent();
        }
    }
}
