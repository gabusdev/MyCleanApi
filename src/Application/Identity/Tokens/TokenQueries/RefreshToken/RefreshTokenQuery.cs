namespace Application.Identity.Tokens.TokenQueries.RefreshToken;

public record RefreshTokenQuery(string Token, string RefreshToken) : IQuery<TokenResponse>;

public class RefreshTokenRequestHandler : IQueryHandler<RefreshTokenQuery, TokenResponse>
{
    private readonly ITokenService _tokenService;
    private readonly IHttpContextService _httpContextService;

    public RefreshTokenRequestHandler(ITokenService tokenService, IHttpContextService httpContextService)
    {
        _tokenService = tokenService;
        _httpContextService = httpContextService;
    }
    public async Task<TokenResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshTokenAsync(request, _httpContextService.GetRequestIpAddress());
    }
}

public class RefreshTokenQueryValidator : AbstractValidator<RefreshTokenQuery>
{
    public RefreshTokenQueryValidator()
    {
        RuleFor(q => q.Token).
            NotEmpty();
        RuleFor(q => q.RefreshToken)
            .NotEmpty();
    }
}