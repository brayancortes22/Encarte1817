using Data.Interfaces;
using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Implements.UserDate
{
    public abstract class AUserData : IUserData
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<User> _dbSet;

        protected AUserData(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public abstract Task<User> LoginAsync(string email, string password);
        public abstract Task<bool> ChangePasswordAsync(int userId, string password);
        public abstract Task<User> GetByEmailAsync(string email);
        public abstract Task SendEmailAsync(string email, string subject, string body);
        public abstract Task<bool> DeleteLogic(bool status);
        public abstract Task<bool> UpdatePartial(User user);
        public abstract Task<bool> AssingRolAsync(string userId, int rolId);

    }
}
