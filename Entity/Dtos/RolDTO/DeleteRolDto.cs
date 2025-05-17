using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.RolDTO
{
    /// <summary>
    /// DTO para la creación de un nuevo rol (operación Delete permanente)
    /// </summary>
    public class DeleteRolDto
    {
        public int Id { get; set; }

    }
}
