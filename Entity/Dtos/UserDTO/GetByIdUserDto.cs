using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para mostrar información básica de un usuario (operación READ)
    /// </summary>
    public class GetByIdUserDto
    {
        public string Id { get; set; } = string.Empty;
    }
}
