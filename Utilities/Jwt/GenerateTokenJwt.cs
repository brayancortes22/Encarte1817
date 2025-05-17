using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entity.Dtos.AuthDTO;
using Entity.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace Utilities.Jwt
{
    public class GenerateTokenJwt
    {
        private IConfiguration _configuration;

        public GenerateTokenJwt(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AuthDto> GeneradorToken(User data)
        {

            // configuracion de los claims
            // la idea es solo mandar el id, por cuestiones de seguidad
            var claims = new List<Claim>
            {
                new Claim("id", data.Id.ToString()),
            };


            // firma del token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            // expiración del token
            var expiracion = DateTime.UtcNow.AddHours(1);

            // Parametros de creacion de token
            var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);

            // retorna el token, y fecha de expiración
            return new AuthDto
            {
                Token = token,
                Expiracion = expiracion
            };
        }

    }
}
