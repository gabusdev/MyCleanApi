using Application.Identity.Users.UserQueries;
using Application.Identity.Users.UserQueries.GetAll;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Queries
{
    public class Query
    {
        public async Task<List<UserDetailsDto>> GetUsers([Service] IMediator mediator,
        CancellationToken ct)
        {
            return await mediator.Send(new GetAllUsersQuery(), ct);
        }
    }

}
