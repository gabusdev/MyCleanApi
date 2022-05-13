using Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.UserQueries.GetAll
{
    public class GetAllUsersQuery: IQuery<List<UserDetailsDto>> 
    {
        public class GetAllUsersQueryHandler : IdentityQueryHandler<GetAllUsersQuery, List<UserDetailsDto>>
        {
            public GetAllUsersQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
            {
            }

            public override async Task<List<UserDetailsDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            {
                return await _userService.GetListAsync(cancellationToken);
            }
        }
    }  
}
