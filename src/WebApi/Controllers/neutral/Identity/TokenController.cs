using Application.Identity.Tokens.Queries;
using Application.Identity.Tokens.Queries.GetToken;
using Application.Identity.Tokens.Queries.RefreshToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.neutral.Identity
{
    [AllowAnonymous]
    public class TokenController : VersionNeutralApiController
    {
        [HttpPost]
        [SwaggerOperation("Login", "Get Auth Token.")]
        public Task<TokenResponse> GetTokenAsync(GetTokenQuery query, CancellationToken cancellationToken)
        {
            return Mediator.Send(query, cancellationToken);
        }

        [HttpPost("refresh-token")]
        [SwaggerOperation("Refresh Token", "Get New Auth Token from Refresh Token.")]
        public Task<TokenResponse> RefreshTokenAsync(RefreshTokenQuery query, CancellationToken cancellationToken)
        {
            return Mediator.Send(query, cancellationToken);
        }
    }
}
