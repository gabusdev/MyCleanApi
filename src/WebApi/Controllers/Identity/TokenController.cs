﻿using Application.Identity.Tokens.TokenQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("Login", "Get Auth Token.")]
        public Task<TokenResponse> GetTokenAsync(GetTokenQuery query, CancellationToken cancellationToken)
        {
            return Mediator.Send(query, cancellationToken);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [SwaggerOperation("Refresh Token", "Get New Auth Token from Refresh Token.")]
        public Task<TokenResponse> RefreshTokenAsync(RefreshTokenQuery query, CancellationToken cancellationToken)
        {
            return Mediator.Send(query, cancellationToken);
        }
    }
}
