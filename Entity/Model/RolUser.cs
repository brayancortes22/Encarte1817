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
        public int UserId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeleteAt { get; set; }
<<<<<<< HEAD
        public required Rol Rol { get; set; }
        public required User User { get; set; }
=======
        public Rol Rol { get; set; }
        public User User { get; set; } 
>>>>>>> d963f4920f4a99c61fefac3e553d4ae2fabee133
    }
}
