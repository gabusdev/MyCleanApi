using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest;

public class EmployeesControllerIntegrationTests : IClassFixture<TestingWebAppFactory<Program>>
{
    private readonly HttpClient _client;
    public EmployeesControllerIntegrationTests(TestingWebAppFactory<Program> factory)
        => _client = factory.CreateClient();
    
    [Fact]
    public async void Index_WhenCalled_ReturnsApplicationForm()
    {
        var response = await _client.GetAsync("/api/v2/test/version");
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        //Assert.Contains("Mark", responseString);
        Assert.Contains("Hola", responseString);
    }
}
