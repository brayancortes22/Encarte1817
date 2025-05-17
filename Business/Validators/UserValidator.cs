// Business/Validators/UserValidator.cs
using Entity.DTOs;
using System.Text.RegularExpressions;
using Business.Validation;

namespace Business.Validators
{
    public class UserValidator : IValidator<UserDto>
    {
        public ValidationResult Validate(UserDto dto)
        {
            var result = new ValidationResult();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.AddError("El nombre es obligatorio");
            }
            else if (dto.Name.Length < 3 || dto.Name.Length > 100)
            {
                result.AddError("El nombre debe tener entre 3 y 100 caracteres");
            }

            // Validar email
            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                result.AddError("El email es obligatorio");
            }
            else if (!IsValidEmail(dto.Email))
            {
                result.AddError("El formato del email no es válido");
            }

            // Validar contraseña (solo en creación, no en actualización)
            if (dto.Id == 0 && string.IsNullOrWhiteSpace(dto.Password))
            {
                result.AddError("La contraseña es obligatoria");
            }
            else if (!string.IsNullOrWhiteSpace(dto.Password) && dto.Password.Length < 6)
            {
                result.AddError("La contraseña debe tener al menos 6 caracteres");
            }

            return result;
        }

        private bool IsValidEmail(string email)
        {
            // Expresión regular simplificada para validar email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}