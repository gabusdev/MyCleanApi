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
        [Error(typeof(FluentValidationException))]
        public async Task<string> InsertUser([Service] IMediator mediator,
        CreateUserCommand input,
        CancellationToken ct)
        {
            return await mediator.Send(input, ct);
        }
    }
    /*public class CreateUserErrorFactory
    : IPayloadErrorFactory<MyCustomErrorA, FluentValidationException>
    {
        public MyCustomErrorA CreateErrorFrom(FluentValidationException ex)
        {
            return new MyCustomError();
        }
    }*/

}
