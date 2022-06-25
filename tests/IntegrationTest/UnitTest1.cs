using Xunit;

namespace WebApi.IntegrationTest;

public class BasicTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BasicTests(CustomWebApplicationFactory<Program> factory)
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
        var responseString = await response.Content.ReadAsStringAsync();
        //Assert.Contains("Mark", responseString);
        Assert.Contains("Hello", responseString);
    }
}
