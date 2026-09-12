using System.Net;
using System.Net.Http.Json;
using Lms_backend.Domain.Constants;
using Lms_backend.Infrastructure;
using Lms_backend.IntegrationTests.Infrastructure;

namespace Lms_backend.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class AuthControllerTests(IntegrationTestWebAppFactory factory) : IAsyncLifetime
{
    private const string SeededUsername = "maria.svensson";
    private const string SeededPassword = UserSeeder.DefaultPassword;
    private const string SeededRole = Roles.Student;

    private readonly HttpClient _client = factory.CreateAuthClient();

    public Task InitializeAsync() => factory.ResetDatabaseAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    private record TokenResponse(string AccessToken);

    private Task<HttpResponseMessage> LoginAsync(string username = SeededUsername, string password = SeededPassword, string role = SeededRole) =>
        _client.PostAsJsonAsync("/api/auth/login", new { username, password, role });

    private static bool HasRefreshTokenCookie(HttpResponseMessage response) =>
        response.Headers.TryGetValues("Set-Cookie", out var cookies) &&
        cookies.Any(c => c.StartsWith("refreshToken=", StringComparison.Ordinal));

    // --- Register ---

    [Fact]
    public async Task Register_WithNewUsername_ReturnsAccessTokenAndSetsRefreshCookie()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new { username = "new.student", password = "P@ssw0rd!" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(HasRefreshTokenCookie(response));

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.False(string.IsNullOrWhiteSpace(body?.AccessToken));
    }

    [Fact]
    public async Task Register_WithExistingUsername_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/auth/register", new { username = SeededUsername, password = "P@ssw0rd!" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // --- Login ---

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsAccessTokenAndSetsRefreshCookie()
    {
        var response = await LoginAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(HasRefreshTokenCookie(response));

        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.False(string.IsNullOrWhiteSpace(body?.AccessToken));
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var response = await LoginAsync(password: "wrong-password");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.False(HasRefreshTokenCookie(response));
    }

    [Fact]
    public async Task Login_WithUnknownUsername_ReturnsUnauthorized()
    {
        var response = await LoginAsync(username: "no.such.user");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // --- Token ---

    [Fact]
    public async Task Token_AfterLogin_ReturnsNewAccessToken()
    {
        var loginResponse = await LoginAsync();
        var loginBody = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

        var response = await _client.PostAsync("/api/auth/token", null);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.False(string.IsNullOrWhiteSpace(body?.AccessToken));
        Assert.NotEqual(loginBody?.AccessToken, body?.AccessToken);
    }

    [Fact]
    public async Task Token_WithoutRefreshCookie_ReturnsUnauthorized()
    {
        var response = await _client.PostAsync("/api/auth/token", null);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // --- Logout ---

    [Fact]
    public async Task Logout_AfterLogin_ClearsRefreshCookieSoTokenNoLongerWorks()
    {
        await LoginAsync();

        var logoutResponse = await _client.PostAsync("/api/auth/logout", null);
        Assert.Equal(HttpStatusCode.OK, logoutResponse.StatusCode);

        var tokenResponse = await _client.PostAsync("/api/auth/token", null);
        Assert.Equal(HttpStatusCode.Unauthorized, tokenResponse.StatusCode);
    }

    // --- GetUser ---

    [Fact]
    public async Task GetUser_AfterLogin_ReturnsAuthenticatedUser()
    {
        var loginResponse = await LoginAsync();
        var loginBody = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();

        var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/getuser");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginBody!.AccessToken);
        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
        Assert.Equal(SeededUsername, user.GetProperty("userName").GetString());
    }

    [Fact]
    public async Task GetUser_WithoutToken_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/auth/getuser");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
