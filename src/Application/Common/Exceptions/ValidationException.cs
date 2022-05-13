using System.Net;

namespace Application.Common.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(List<string>? errors = default)
        : base("Validation Errors Occurred.", errors, HttpStatusCode.BadRequest) { }

    }
}
