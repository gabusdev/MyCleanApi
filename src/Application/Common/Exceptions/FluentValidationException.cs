using FluentValidation.Results;

namespace Application.Common.Exceptions
{
    internal class FluentValidationException : ValidationException
    {
        public FluentValidationException(string message, List<string> errors)
            : base(message, errors) { }
        public FluentValidationException(string message, IEnumerable<ValidationFailure> failures)
            : this
            (message, failures.Select(f => $"{f.ErrorMessage}").ToList())
        { }


    }
}
