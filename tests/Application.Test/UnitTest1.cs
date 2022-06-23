using Application.Common.Caching;
using Application.Common.Exporters;
using Application.Common.FileStorage;
using Application.Common.Persistence;
using Application.Identity.Users;
using Application.Identity.Users.Queries;
using Infrastructure;
using Infrastructure.FileStorage;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Xunit;

namespace Application.UnitTest
{
    public class Startup
    {
        public void ConfigureHost(IHostBuilder hostBuilder) =>
        hostBuilder
            .ConfigureHostConfiguration(builder => { })
            .ConfigureAppConfiguration((context, builder) => {
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            });

        public void ConfigureServices(IServiceCollection services, HostBuilderContext context)
        {
            var c = context.Configuration;
            services.AddInfrastructure(c);
        }
    }
    public class UnitTest1
    {
        private readonly IUserService _userservice;
        public UnitTest1(IUserService userservice)
        {
            _userservice = userservice;
        }
        [Fact]
        public async void PostLoginIsSuccessfull()
        {
            var result = await _userservice.GetAsync(new System.Threading.CancellationToken());

            Assert.IsType<List<UserDetailsDto>>(result);
        }
    }
    public class IntegrationTest
    {
        private readonly WebApi.Controllers.v2.TestController _cotroller;
        public IntegrationTest(ICacheService cacheService, IDapperService ds,
            IFileStorageService fs, IExcelWriter ews)
        {
            _cotroller = new WebApi.Controllers.v2.TestController(cacheService,
                ds, fs, ews);
        }
        [Fact]
        public void TestTest()
        {
            var result = _cotroller.SayVersion();

            Assert.Equal("Hello from Version 2", result);
        }
    }
}