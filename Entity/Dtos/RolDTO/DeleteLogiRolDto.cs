using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.RolDTO
{
    /// <summary>
    /// DTO para la creación de un nuevo rol (operación Delete Logico)
    /// </summary>
    public class DeleteLogiRolDto
    {
        public int Id { get; set; }
        public required bool Status { get; set; } = false;

    }
}
