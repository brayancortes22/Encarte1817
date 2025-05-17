using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entity.Dtos.CredencialesDTO
{
    public class CredencialesDto
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
}
