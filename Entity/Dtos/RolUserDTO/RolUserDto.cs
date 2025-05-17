using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos.RolUserDTO
{
    /// <summary>
    /// DTO para mostrar información básica de una asignación de rol a usuario (operación GET ALL, CREATE, UPDATE(PATCH-PUT))
    /// </summary>
    public class RolUserDto
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
    }
}
