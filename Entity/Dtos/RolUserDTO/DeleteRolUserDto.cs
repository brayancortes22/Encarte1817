using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.RolUserDTO
{
    /// <summary>
    /// DTO para la creación de una nueva asignación de rol a usuario (operación DELETE permanente)
    /// </summary>
    public class DeleteRolUserDto
    {
        public int Id { get; set; }
    }
}
