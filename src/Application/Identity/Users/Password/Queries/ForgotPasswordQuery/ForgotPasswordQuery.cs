namespace Application.Identity.Users.Password.Queries.ForgotPasswordQuery;

public class ForgotPasswordQuery : IQuery<string>
{
    public string Email { get; set; } = default!;

    public class ForgotPasswordQueryHandler : IdentityQueryHandler<ForgotPasswordQuery, string>
    {
        public ForgotPasswordQueryHandler(IUserService userService, IHttpContextService httpContextService) : base(userService, httpContextService)
        {
        }

        public override async Task<string> Handle(ForgotPasswordQuery request, CancellationToken cancellationToken)
        {
            var token = await _userService.ForgotPasswordAsync(request);
            return token;
        }
    }
}

public class ForgotPasswordQueryValidator : AbstractValidator<ForgotPasswordQuery>
{
    public ForgotPasswordQueryValidator() =>
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage("Invalid Email Address.");
}