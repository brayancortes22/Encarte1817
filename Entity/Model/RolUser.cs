using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Model
{
    public class RolUser
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public string UserId { get; set; }= string.Empty;
        public bool Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
        public Rol Rol { get; set; }
        public User User { get; set; } 
    }
}
