using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Dtos.RolUserDTO
{
    public class RolUserDto
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string UserId { get; set; }= string.Empty;
        public bool Status { get; set; }
    }
}
