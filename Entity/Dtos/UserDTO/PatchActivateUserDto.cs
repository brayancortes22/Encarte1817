using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.UserDTO
{
    /// <summary>
    /// DTO para actualizar solo el estado de un usuario (operación patch de activar parcial)
    /// </summary>
    public class PatchActivateUserDto
    {
        public required string Id { get; set; }
        public bool Status { get; set; }= true;
    }
}
