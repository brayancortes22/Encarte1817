using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Model;

namespace Data.Interfaces
{
    public interface IUserData
    {
        Task<User> LoginAsync(string email, string password);
        Task<User> ChangePasswordAsync(int userId,string password);
        Task<User> SendEmailAsync(string email);
    }
}
