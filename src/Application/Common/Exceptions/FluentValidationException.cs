using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Exceptions
{
    internal class FluentValidationException : ValidationException
    {
        public FluentValidationException(List<string> errors)
            : base(errors) { }
        public FluentValidationException(IEnumerable<ValidationFailure> failures)
            : this
            (failures.Select(f => $"{f.ErrorMessage}").ToList()) { }
            

    }
}
