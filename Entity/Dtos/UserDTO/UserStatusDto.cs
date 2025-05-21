using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para actualizar el estado de activación de un usuario
    /// </summary>
    public class UserStatusDto
    {
        /// <summary>
        /// Indica si el usuario debe estar activo (true) o inactivo (false)
        /// </summary>
        public bool IsActive { get; set; }
    }
}