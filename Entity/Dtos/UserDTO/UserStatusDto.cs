using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para actualizar el estado (activo/inactivo) de un usuario
    /// </summary>
    public class UserStatusDto
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El estado del usuario es obligatorio.")]
        public bool Status { get; set; }
    }
}
