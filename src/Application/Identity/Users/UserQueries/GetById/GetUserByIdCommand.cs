using Application.Common.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Identity.Users.UserQueries.GetById
{
    public class GetUserByIdQuery: IQuery<UserDetailsDto>
    {
        public string UserId { get; set; } = null!;

        public class GetUserByIdQueryHandler : IdentityQueryHandler<GetUserByIdQuery, UserDetailsDto>
        {
            public GetUserByIdQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
            {
            }

            public override async Task<UserDetailsDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                return await _userService.GetAsync(request.UserId, cancellationToken);
            }
        }
        public class GetUserByIdQueryValidator: AbstractValidator<GetUserByIdQuery>
        {
            public GetUserByIdQueryValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();
            }
        }
    }
}
