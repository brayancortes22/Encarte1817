using System.Collections.Generic;
using System.Linq;

namespace Business.Validation
{
    public class ValidationResult
    {
        private readonly List<string> _errors = new();

        public bool IsValid => !_errors.Any();
        public IReadOnlyList<string> Errors => _errors.AsReadOnly();

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            _errors.AddRange(errors);
        }
    }
}