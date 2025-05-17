using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para mostrar información básica de un usuario (operación get all, create, update(patch-put))
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool Status { get; set; }
    }
}
