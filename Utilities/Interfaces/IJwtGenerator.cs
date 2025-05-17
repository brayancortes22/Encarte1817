using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Dtos.AuthDTO;
using Entity.Model;

namespace Utilities.Interfaces
{
        /// <summary>
        /// Interfaz para la generación de tokens JWT.
        /// Define el contrato para la creación de un token a partir de datos de usuario.
        /// </summary>
        public interface IJwtGenerator
        {
        /// <summary>
        /// Genera un token JWT con los datos del usuario.
        /// </summary>
        Task<AuthDto> GeneradorToken(User user);
    }

    }







