namespace Business.Validation
{
    public interface IValidator<TDto>
    {
        ValidationResult Validate(TDto dto);
    }
}