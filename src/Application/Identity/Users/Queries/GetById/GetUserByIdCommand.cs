namespace Application.Identity.Users.Queries.GetById
{
    public class GetUserByIdQuery : IQuery<UserDetailsDto?>
    {
        public string? UserId { get; set; }

        public class GetUserByIdQueryHandler : IdentityQueryHandler<GetUserByIdQuery, UserDetailsDto?>
        {
            public GetUserByIdQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
            {
            }

            public override async Task<UserDetailsDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            {
                return await _userService.GetByIdAsync(request.UserId!, cancellationToken);
            }
        }
        public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
        {
            public GetUserByIdQueryValidator()
            {
                RuleFor(x => x.UserId)
                    .NotEmpty();
            }
        }
    }
}
