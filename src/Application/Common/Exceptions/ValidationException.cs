using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(List<string>? errors = default)
        : base("Validation Errors Occurred.", errors, HttpStatusCode.BadRequest) { }

    }
}
