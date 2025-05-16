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
        Task<bool> ChangePasswordAsync(int userId,string password);
        Task SendEmailAsync(string email);
        Task<bool> DeleteLogic(bool status);
        Task<bool> UpdatePartial(User user);
        Task<bool> AssingRolAsync(string userId, int rolId);
    }
}
