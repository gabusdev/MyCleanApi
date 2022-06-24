using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    [Fact]
    public async void Index_WhenCalled_ReturnsApplicationForm()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/v2/test/versions");
        Assert.NotNull(response);
        response.EnsureSuccessStatusCode();
        //var responseString = await response.Content.ReadAsStringAsync();
        //Assert.Contains("Mark", responseString);
        //Assert.Contains("Hola", responseString);
    }
}
