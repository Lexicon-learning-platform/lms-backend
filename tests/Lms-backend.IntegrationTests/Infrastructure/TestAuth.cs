using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Lms_backend.IntegrationTests.Infrastructure;

public static class TestAuth
{
    private record TokenResponse(string AccessToken);

    public static async Task<string> LoginAsync(HttpClient client, string username, string password)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new { username, password });
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return body!.AccessToken;
    }

    public static async Task<HttpClient> CreateAuthenticatedClientAsync(IntegrationTestWebAppFactory factory, string username, string password)
    {
        var client = factory.CreateAuthClient();
        var token = await LoginAsync(client, username, password);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }
}
