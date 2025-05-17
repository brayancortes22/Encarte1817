// Business/Validators/RolValidator.cs
using Entity.DTOs;
using Business.Validation;

namespace Business.Validators
{
    public class RolValidator : IValidator<RolDto>
    {
        public ValidationResult Validate(RolDto dto)
        {
            var result = new ValidationResult();

            // Validar nombre
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.AddError("El nombre del rol es obligatorio");
            }
            else if (dto.Name.Length < 2 || dto.Name.Length > 50)
            {
                result.AddError("El nombre del rol debe tener entre 2 y 50 caracteres");
            }

            // Validar descripción (opcional)
            if (!string.IsNullOrWhiteSpace(dto.Description) && dto.Description.Length > 200)
            {
                result.AddError("La descripción no puede exceder los 200 caracteres");
            }

            return result;
        }
    }
}