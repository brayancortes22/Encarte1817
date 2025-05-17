using Entity.Context;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilities.Mail;

namespace Data.Implements.UserDate
{
    public class UserData : AUserData
    {
        private readonly SwtpSettings _swtpSettings;

        public UserData(ApplicationDbContext context, SwtpSettings swtpSettings) : base(context)
        {
            _swtpSettings = swtpSettings; // Inicializar el campo
        }

        public override async Task<User> LoginAsync(string email, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        public override async Task<bool> ChangePasswordAsync(int userId, string password)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            user.Password = password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public override async Task<User> GetByEmailAsync(string email)
        {
            // Replace the call to GetAllAsync with a proper DbSet query
            var users = await _context.Users.ToListAsync(); // Correct usage of DbSet
            return users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public override async Task SendEmailAsync(string email, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El correo de destino no puede estar vacío.");

            // Configurar el mensaje
            var message = new MailMessage
            {
                From = new MailAddress(_swtpSettings.SenderEmail, _swtpSettings.SenderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            message.To.Add(new MailAddress(email));

            // Configurar el cliente SMTP
            using var client = new SmtpClient(_swtpSettings.Server, _swtpSettings.Port)
            {
                Credentials = new NetworkCredential(_swtpSettings.UserName, _swtpSettings.Password),
                EnableSsl = _swtpSettings.EnableSsl,
                Timeout = _swtpSettings.TimeOut
            };

            await client.SendMailAsync(message);
        }

        public override async Task<bool> DeleteLogic(bool status)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Status == status);
            if (user == null) return false;
            user.Status = !status;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public override async Task<bool> UpdatePartial(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null) return false;
            // Update only the fields that are not null
            if (!string.IsNullOrEmpty(user.Email)) existingUser.Email = user.Email;
            if (!string.IsNullOrEmpty(user.Password)) existingUser.Password = user.Password;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public override async Task<bool> AssingRolAsync(string userId, int rolId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            user.RoleId = rolId;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
