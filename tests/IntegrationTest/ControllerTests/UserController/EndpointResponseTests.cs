using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.IntegrationTest.ControllerTests.UserController
{
    public class EndpointResponseTests : BaseTest
    {
        public EndpointResponseTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            Route = "/api/v1/user";
        }
    }
}
