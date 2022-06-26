using Application.Identity.Tokens.Queries;
using Application.Identity.Users.Queries;
using System.Collections.Generic;

namespace WebApi.IntegrationTest;

public class BaseTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    protected readonly CustomWebApplicationFactory<Program> _factory;

    protected string Route { get; set; }
    protected HttpClient _client;

    public BaseTest(CustomWebApplicationFactory<Program> factory, string? route = null)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        Route = route ?? "/";
    }
    /// <summary>
    /// Tries to Login a user with the specified email and password
    /// </summary>
    /// <param name="mail">Email to use for login</param>
    /// <param name="pass">Password to use for login</param>
    /// <returns>A touple with the token and the Id for the User</returns>
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
        
        return (token.Token, user.Id ?? "");
    }
    /// <summary>
    /// Adds the <c>Authorization</c> Header for the passed <see cref="HttpClient"/>
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/> client</param>
    /// <param name="token">The token for <c>Authorization</c> Header</param>
    protected static void AuthorizeClient (HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }
}
