using Application.Identity.Tokens.TokenQueries;
using Application.Identity.Tokens.TokenQueries.GetToken;
using Application.Identity.Tokens.TokenQueries.RefreshToken;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v1.Identity
{
    [AllowAnonymous]
    [HttpCacheIgnore]
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
