using Application.Common.Caching;
using Application.Common.Exporters;
using Application.Common.FileStorage;
using Application.Common.Persistence;
using Application.Identity.Users.UserQueries;
using Application.UserNotifications.Queries.GetUnreadedNotifications;
using Domain.Common;
using Infrastructure.Identity.User;
using Mapster;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers.v2
{
    [ApiVersion("2")]
    public class TestController : VersionedApiController
    {
        private readonly ICacheService _cache;
        private readonly IDapperService _dapper;
        private readonly IFileStorageService _fService;
        private readonly IExcelWriter _excelWriter;
        public TestController(ICacheService cache, IDapperService dapper, IFileStorageService fService,
            IExcelWriter excelWriter)
        {
            _cache = cache;
            _dapper = dapper;
            _fService = fService;
            _excelWriter = excelWriter;
        }

        [HttpGet("versions")]
        public string SayVersion()
        {
            return "Hello from Version 2";
        }
        [HttpGet("cache")]
        [HttpCacheIgnore]
        public async Task<string> CaheTest()
        {
            return await _cache.GetOrSetAsync("testeo", async () => await Task.Delay(5000).ContinueWith((t) => "Hola"));
        }
        [HttpGet("dapper")]
        //[ApiResponseCache(30,ResponseCacheLocation.Any)]
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Private, MaxAge = 15)]
        public async Task<ActionResult> TestDapper()
        {
            var query = "select*from AspNetUsers u where u.Email = @mail";
            var param = new { mail = "admin@mail.com" };
            var result = await _dapper.QueryFirstOrDefaultAsync<ApplicationUser>(query, param);
            Log.Information(">>>>>>> Using Dapper");
            return Ok(result);
        }
        [HttpPost("upload-image")]
        public async Task<string> Upload(FileUploadRequest req)
        {
            var result = await _fService.UploadAsync<string>(req, FileType.Image);
            return result;
        }

        [HttpGet("test-excel")]
        public async Task<FileResult> Excel()
        {
            var x = await _dapper.QueryAsync<ApplicationUser>("select * from AspNetUsers");
            var result = _excelWriter.WriteToStream(x.Adapt<List<UserDetailsDto>>());
            return File(result, "application/octet-stream", "UsersData.xlsx");
        }

        [HttpGet("notifications-test/{userId}")]
        public async Task<ActionResult> NotificationsTest(string userId)
        {
            var response = await Mediator.Send(new GetUnreadedNotificationsByUserIdQuery { UserId = userId });
            return Ok(response);
        }
    }
}
