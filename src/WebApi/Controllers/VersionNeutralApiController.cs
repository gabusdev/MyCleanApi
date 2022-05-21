using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiVersionNeutral]
    public class VersionNeutralApiController : BaseApiController
    {
    }
}
