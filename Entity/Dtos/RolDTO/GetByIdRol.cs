using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.RolDTO
{
    /// <summary>
    /// DTO para la creación de un nuevo rol (operación get all)
    /// </summary>
    public class GetByIdRolDto
    {
        public int Id { get; set; }
    }
}
