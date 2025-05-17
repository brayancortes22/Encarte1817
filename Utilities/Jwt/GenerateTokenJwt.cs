using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entity.Dtos.AuthDTO;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Utilities.Interfaces;

namespace Utilities.Jwt
{
    /// <summary>
    /// Clase encargada de generar tokens JWT para autenticación.
    /// </summary>
    public class GenerateTokenJwt : IJwtGenerator
    {
        private readonly IConfiguration _configuration;


        /// <summary>
        /// Constructor que inyecta la configuración del proyecto para acceder a clave del archivo appsettings.json.
        /// </summary>
        /// <param name="configuration">Instancia de IConfiguration.</param>
        public GenerateTokenJwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Genera un token JWT basado en la información del usuario.
        /// </summary>
        /// <param name="data">Instancia del modelo User con la información del usuario autenticado.</param>
        /// <returns>Un objeto AuthDto que contiene el token y la fecha de expiración.</returns>
        public async Task<AuthDto> GeneradorToken(User data)
        {
            


            // 1. Claims (datos incluidos en el token)
            // En este caso solo el ID del usuario para evitar exposición de datos sensibles
            var claims = new List<Claim>
            {
                new Claim("id", data.Id.ToString())
                // Puedes agregar más claims si es necesario, como roles, permisos, etc.
                // new Claim(ClaimTypes.Role, data.Role),
            };

            
            // 2. Firma del Token
            // Se obtiene la clave secreta desde appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));

            // Se define el algoritmo de firma (HMAC SHA-256)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);




            // 3. Definir la expiración del token
            // Aquí se configura una duración de 1 hora.
            var expiracion = DateTime.UtcNow.AddHours(1);



            // 4. Crear el Token JWT
            var tokenSeguridad = new JwtSecurityToken(
                issuer: null,               // Puede configurarse si se requiere
                audience: null,             // Puede configurarse si se requiere
                claims: claims,             // Los datos incluidos
                expires: expiracion,        // Fecha de expiración
                signingCredentials: creds   // Firma
            );

            

            // 5. Serializar el token
            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);

            
            // 6. Retornar DTO con token y expiración
            return new AuthDto
            {
                Token = token,
                Expiracion = expiracion
            };
        }
    }
}

