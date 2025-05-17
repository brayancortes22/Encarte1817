using System;
using System.Text.RegularExpressions;
using Entity.Model;
using Entity.Dtos.UserDTO;

namespace Utilities.Helpers
{
    public interface IUserHelper
    {
        bool IsValidEmail(string email);
        bool IsValidUsername(string username);
        string NormalizeUsername(string username);
        string GetUserFullName(UserDto user);
        string GetUserInitials(UserDto user);
    }

    public class UserHelper : IUserHelper
    {
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular simple para validar emails
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        public bool IsValidUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return false;

            // Solo permitir letras, números, guiones y guiones bajos
            var regex = new Regex(@"^[a-zA-Z0-9_-]{3,20}$");
            return regex.IsMatch(username);
        }

        public string NormalizeUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return string.Empty;

            // Convertir a minúsculas y eliminar espacios en blanco
            return username.ToLowerInvariant().Trim();
        }

        public string GetUserFullName(UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.LastName))
                return $"{user.FirstName} {user.LastName}";

            if (!string.IsNullOrWhiteSpace(user.FirstName))
                return user.FirstName;

            if (!string.IsNullOrWhiteSpace(user.LastName))
                return user.LastName;

            return user.Username ?? "Anonymous User";
        }

        public string GetUserInitials(UserDto user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            string initials = string.Empty;

            if (!string.IsNullOrWhiteSpace(user.FirstName) && user.FirstName.Length > 0)
                initials += user.FirstName[0];

            if (!string.IsNullOrWhiteSpace(user.LastName) && user.LastName.Length > 0)
                initials += user.LastName[0];

            if (string.IsNullOrWhiteSpace(initials) && !string.IsNullOrWhiteSpace(user.Username) && user.Username.Length > 0)
                initials = user.Username[0].ToString().ToUpper();

            return string.IsNullOrWhiteSpace(initials) ? "U" : initials.ToUpper();
        }
    }
}