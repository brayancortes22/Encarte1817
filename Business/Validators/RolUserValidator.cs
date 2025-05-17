// Business/Validators/RolUserValidator.cs
using Entity.DTOs;
using Business.Validation;

namespace Business.Validators
{
    public class RolUserValidator : IValidator<RolUserDto>
    {
        public ValidationResult Validate(RolUserDto dto)
        {
            var result = new ValidationResult();

            // Validar UserId
            if (dto.UserId <= 0)
            {
                result.AddError("El ID de usuario debe ser mayor que cero");
            }

            // Validar RolId
            if (dto.RolId <= 0)
            {
                result.AddError("El ID de rol debe ser mayor que cero");
            }

            return result;
        }
    }
}