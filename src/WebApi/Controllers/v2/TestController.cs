using Application.Common.Caching;
using Application.Common.Exporters;
using Application.Common.FileStorage;
using Application.Common.Persistence;
using Domain.Common;
using Infrastructure.Identity.User;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> TestDapper()
        {
            var query = "select*from AspNetUsers u where u.Email = @mail";
            var param = new { mail = "admin@mail.com" };
            var result = await _dapper.QueryFirstOrDefaultAsync<ApplicationUser>(query, param);
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
            var result = _excelWriter.WriteToStream(x.ToList());
            return File(result, "application/octet-stream", "ProductExports.xlsx");
        }
    }
}
