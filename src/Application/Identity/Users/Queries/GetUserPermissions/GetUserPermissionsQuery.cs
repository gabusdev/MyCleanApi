namespace Application.Identity.Users.Queries.GetUserPermissions
{
    public class GetUserPermissionsQuery : IQuery<List<string>>
    {
        public string? UserId { get; set; }

        public class GetUserPermissionsQueryHandler : IdentityQueryHandler<GetUserPermissionsQuery, List<string>>
        {
            public GetUserPermissionsQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
            {
            }

            public override async Task<List<string>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
            {
                if (request.UserId is not null &&
                   await _userService.GetByIdAsync(request.UserId, cancellationToken) is var user)
                {
                    return await _userService.GetPermissionsAsync(request.UserId, cancellationToken);
                }
                throw new NotFoundException("User not Found");
            }
        }

        public class GetUserPermissionsQueryValidator : AbstractValidator<GetUserPermissionsQuery>
        {
            public GetUserPermissionsQueryValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();
            }
        }
    }
}
