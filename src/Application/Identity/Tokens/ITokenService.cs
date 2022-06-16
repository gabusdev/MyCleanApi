using Application.Identity.Tokens.Queries;
using Application.Identity.Tokens.Queries.GetToken;
using Application.Identity.Tokens.Queries.RefreshToken;

namespace Application.Identity.Tokens;

public interface ITokenService
{
    Task<TokenResponse> GetTokenAsync(GetTokenQuery request, string ipAddress, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenQuery request, string ipAddress);
}