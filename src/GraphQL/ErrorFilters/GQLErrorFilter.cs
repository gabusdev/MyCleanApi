using Application.Common.Exceptions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.ErrorFilters;

public class GQLErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        var exception = error.Exception;
        switch (exception)
        {
            case CustomException e:
                error = error.RemoveException();
                error = error.WithMessage(e.Message);
                error = error.WithCode(Convert.ToString((int)e.StatusCode));
                if (e.ErrorMessages is not null)
                {
                    error.SetExtension("Errors", e.ErrorMessages);
                }
                break;

            case KeyNotFoundException:
                error = error.RemoveException();
                error = error.WithMessage("Not Found");
                error = error.WithCode("404");
                break;

            default:
                error = error.WithMessage("Internal Error");
                error = error.WithCode("500");
                break;
        }

        error = error.RemoveLocations();
        error = error.RemovePath();
        error = error.RemoveSyntaxNode();

        Log.Error($"GraphQL Exception: {nameof(exception)}. Request failed with code: {error.Code}");

        return error;
    }
}

