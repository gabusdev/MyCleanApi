using Application.Identity.Tokens.TokenQueries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        [HttpPost]
        public Task<TokenResponse> GetTokenAsync(GetTokenQuery query, CancellationToken cancellationToken)
        {
            return Mediator.Send(query,cancellationToken);
        }

        [HttpPost("refresh-token")]
        public Task<TokenResponse> RefreshTokenAsync(RefreshTokenQuery query, CancellationToken cancellationToken)
        {
            return Mediator.Send(query, cancellationToken);
        }
    }
}
