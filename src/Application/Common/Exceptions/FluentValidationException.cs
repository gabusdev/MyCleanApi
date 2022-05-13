using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    internal class FluentValidationException : ValidationException
    {
        public FluentValidationException(List<string> errors)
            : base(errors) { }
        public FluentValidationException(IEnumerable<ValidationFailure> failures)
            : this
            (failures.Select(f => $"{f.ErrorMessage}").ToList())
        { }


    }
}
