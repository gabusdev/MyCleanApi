using Application.Identity.Tokens.TokenQueries;
using Application.Identity.Tokens.TokenQueries.GetToken;
using Application.Identity.Tokens.TokenQueries.RefreshToken;

namespace Application.Identity.Tokens;

public interface ITokenService
{
    Task<TokenResponse> GetTokenAsync(GetTokenQuery request, string ipAddress, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenQuery request, string ipAddress);
}