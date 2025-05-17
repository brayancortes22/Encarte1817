using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos.RolDTO
{
    /// <summary>
    /// DTO para mostrar información básica de un rol (operación GET ALL,CREATE,UPDATE(PATCH-PUT))
    /// </summary>
    public class RolDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public bool Status { get; set; }
    }
}
