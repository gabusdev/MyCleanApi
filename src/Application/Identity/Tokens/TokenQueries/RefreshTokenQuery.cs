using Application.Common.Messaging;

namespace Application.Identity.Tokens.TokenQueries;

public record RefreshTokenQuery(string Token, string RefreshToken) : IQuery<TokenResponse>;

public class RefreshTokenRequestHandler : IQueryHandler<RefreshTokenQuery, TokenResponse>
{
    private readonly ITokenService _tokenService;

    public RefreshTokenRequestHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    public async Task<TokenResponse> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
    {
        return await _tokenService.RefreshTokenAsync(request, "");
    }
}