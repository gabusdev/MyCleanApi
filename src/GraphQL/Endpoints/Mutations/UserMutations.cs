using Application.Common.Exceptions;
using Application.Identity.Users.Commands.CreateUser;
using MediatR;

namespace GraphQL.Endpoints.Mutations
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

    {
        public CustomGQLError CreateErrorFrom(FluentValidationException ex)
        {
            return new CustomGQLError()
            {
                Message = ex.Message,
                Errors = ex.ErrorMessages,
                StatusCode = (int)ex.StatusCode
            };
        }
        public CustomGQLError CreateErrorFrom(Exception ex)
        {
            return new CustomGQLError()
            {
                Message = "An Unesperated Error Occured",
                Errors = new() { ex.Message },
                StatusCode = 500
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
