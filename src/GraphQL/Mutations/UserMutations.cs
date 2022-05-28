using Application.Common.Exceptions;
using Application.Identity.Users.UserCommands.CreateUser;
using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Queries
{
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class UserMutations
    {
        [UseMutationConvention]
        [Error(typeof(CreateUserErrorFactory))]
        public async Task<string> InsertUser(CreateUserCommand input,
            [Service] IMediator mediator,
            CancellationToken ct)
        {
            return await mediator.Send(input, ct);
        }
    }
    public class CreateUserErrorFactory
    : IPayloadErrorFactory<FluentValidationException, CustomGQLError>
    {
        public CustomGQLError CreateErrorFrom(FluentValidationException exception)
        {
            return new CustomGQLError()
            {
                Message = exception.Message,
                Errors = exception.ErrorMessages,
                StatusCode = (int)exception.StatusCode
            };
        }
    }
    public class CustomGQLError
    {
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public int StatusCode { get; set; }

    }
}
