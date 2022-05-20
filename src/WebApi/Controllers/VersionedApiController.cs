using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VersionedApiController : BaseApiController
    {
    }
}
