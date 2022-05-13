using Application.Common.Messaging;

namespace Application.Identity.Tokens.TokenQueries;

public record GetTokenQuery(string Email, string Password): IQuery<TokenResponse>;

public class GetTokenQueryHandler : IQueryHandler<GetTokenQuery, TokenResponse>
{
    private readonly ITokenService _tokenService;

    public GetTokenQueryHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    public async Task<TokenResponse> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        return await _tokenService.GetTokenAsync(request,"",cancellationToken);
    }
}

public class TokenRequestValidator : AbstractValidator<GetTokenQuery>
{
    public TokenRequestValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress()
                .WithMessage("Invalid Email Address.");

        RuleFor(p => p.Password).Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}