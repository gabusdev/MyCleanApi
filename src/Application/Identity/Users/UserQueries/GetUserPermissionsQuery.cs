﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.UserQueries
{
    public class GetUserPermissionsQuery: IQuery<List<string>>
    {
        public string? UserId { get; set; }

        public class GetUserPermissionsQueryHandler : IdentityQueryHandler<GetUserPermissionsQuery, List<string>>
        {
            public GetUserPermissionsQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
            {
            }

            public async override Task<List<string>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
            {
                if(request.UserId is not null &&
                   await _userService.GetAsync(request.UserId, cancellationToken) is var user)
                {
                    return await _userService.GetPermissionsAsync(request.UserId, cancellationToken);
                }
                throw new NotFoundException("User not Found");
            }
        }
    }
}