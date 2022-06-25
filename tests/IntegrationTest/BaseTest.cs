using Application.Identity.Tokens.Queries;
using Application.Identity.Users.Queries;
using System.Collections.Generic;

namespace WebApi.IntegrationTest;

public class BaseTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly CustomWebApplicationFactory<Program> _factory;

    public BaseTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    protected async Task<(string, string)> TryLogin(string mail = "admin@mail.com", string pass = "admin")
    {
        var client = _factory.CreateClient();
        
        var formModel = new Dictionary<string, string>
        {
            {"email", mail },
            {"password", pass}
        };

        var response = await client.PostAsJsonAsync("/api/v1/token", formModel);
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadAsAsync<TokenResponse>();

        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token.Token);
        response = await client.GetAsync("/api/v1/profile");
        response.EnsureSuccessStatusCode();
        var user = await response.Content.ReadAsAsync<UserDetailsDto>();
        
        return (token.Token, user.Id!);
    }
}
