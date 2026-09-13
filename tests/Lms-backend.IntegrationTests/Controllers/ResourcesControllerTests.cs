using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Lms_backend.Infrastructure;
using Lms_backend.IntegrationTests.Infrastructure;

namespace Lms_backend.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class ResourcesControllerTests(IntegrationTestWebAppFactory factory) : IAsyncLifetime
{
    private const string OwnerUsername = "maria.svensson";
    private const string OtherUsername = "johan.berg";

    private HttpClient _anonymousClient = null!;
    private HttpClient _ownerClient = null!;
    private HttpClient _otherClient = null!;
    private HttpClient _adminClient = null!;

    public async Task InitializeAsync()
    {
        await factory.ResetDatabaseAsync();
        _anonymousClient = factory.CreateAuthClient();
        _ownerClient = await TestAuth.CreateAuthenticatedClientAsync(factory, OwnerUsername, UserSeeder.DefaultPassword);
        _otherClient = await TestAuth.CreateAuthenticatedClientAsync(factory, OtherUsername, UserSeeder.DefaultPassword);
        _adminClient = await TestAuth.CreateAuthenticatedClientAsync(factory, "admin", UserSeeder.DefaultPassword);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static Guid OwnerId => TestUsers.Seeded.Single(u => u.UserName == OwnerUsername).Id;

    private static object ValidResourceBody(string name = "Syllabus", int type = 0, string data = "Some text content.") => new
    {
        name,
        description = "A resource created for integration tests.",
        type,
        data,
    };

    private async Task<JsonElement> CreateResourceAsync(HttpClient client, string name = "Syllabus", int type = 0)
    {
        var response = await client.PostAsJsonAsync("/api/resources", ValidResourceBody(name: name, type: type));
        return await response.Content.ReadFromJsonAsync<JsonElement>();
    }

    // --- Create ---

    [Fact]
    public async Task CreateResource_AsAuthenticatedUser_ReturnsCreatedWithLocation()
    {
        var response = await _ownerClient.PostAsJsonAsync("/api/resources", ValidResourceBody());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Syllabus", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task CreateResource_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.PostAsJsonAsync("/api/resources", ValidResourceBody());

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateResource_WithTooShortName_ReturnsBadRequest()
    {
        var response = await _ownerClient.PostAsJsonAsync("/api/resources", ValidResourceBody(name: "ab"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateResource_SetsCurrentUserAsOwner()
    {
        var created = await CreateResourceAsync(_ownerClient);

        Assert.Equal(OwnerId.ToString(), created.GetProperty("createdBy").GetProperty("id").GetString());
    }

    // --- Get ---

    [Fact]
    public async Task GetResource_AsAuthenticatedUser_ReturnsResource()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _otherClient.GetAsync($"/api/resources/{id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Syllabus", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetResource_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.GetAsync($"/api/resources/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetResource_WithUnknownId_ReturnsNotFound()
    {
        var response = await _ownerClient.GetAsync($"/api/resources/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetResources_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.GetAsync("/api/resources");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetResources_WithoutTypeFilter_ReturnsResourcesOfAllTypes()
    {
        // Regression test: type used to be a non-nullable query param, so omitting it silently
        // filtered to ResourceType.Text (0) only instead of returning everything.
        await CreateResourceAsync(_ownerClient, name: "Text Resource", type: 0);
        await CreateResourceAsync(_ownerClient, name: "Url Resource", type: 1);

        var response = await _ownerClient.GetAsync("/api/resources");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var names = body.EnumerateArray().Select(r => r.GetProperty("name").GetString()).ToList();
        Assert.Contains("Text Resource", names);
        Assert.Contains("Url Resource", names);
    }

    [Fact]
    public async Task GetResources_FiltersByType()
    {
        await CreateResourceAsync(_ownerClient, name: "Text Resource", type: 0);
        await CreateResourceAsync(_ownerClient, name: "Url Resource", type: 1);

        var response = await _ownerClient.GetAsync("/api/resources?type=URL");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var resources = body.EnumerateArray().ToList();
        Assert.Single(resources);
        Assert.Equal("Url Resource", resources[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetResources_RespectsSmallPageSize()
    {
        // Regression test: pageSize used to be clamped up to 10 whenever it was <= 10 instead
        // of only when <= 0, so a request for pageSize=1 silently returned 10 items.
        await CreateResourceAsync(_ownerClient, name: "Resource A");
        await CreateResourceAsync(_ownerClient, name: "Resource B");
        await CreateResourceAsync(_ownerClient, name: "Resource C");

        var response = await _ownerClient.GetAsync("/api/resources?pageSize=1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Single(body.EnumerateArray());
    }

    [Fact]
    public async Task GetResources_IncludesPaginationHeader()
    {
        await CreateResourceAsync(_ownerClient);

        var response = await _ownerClient.GetAsync("/api/resources");

        Assert.True(response.Headers.TryGetValues("X-Pagination", out _));
    }

    // --- Update ---

    [Fact]
    public async Task UpdateResource_AsOwner_PersistsChanges()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _ownerClient.PutAsJsonAsync($"/api/resources/{id}", ValidResourceBody(name: "Renamed Resource"));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _ownerClient.GetAsync($"/api/resources/{id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Renamed Resource", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task UpdateResource_AsNonOwnerNonAdmin_ReturnsForbidden()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _otherClient.PutAsJsonAsync($"/api/resources/{id}", ValidResourceBody(name: "Hijacked"));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateResource_AsAdmin_CanModifyAnyResource()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _adminClient.PutAsJsonAsync($"/api/resources/{id}", ValidResourceBody(name: "Moderated"));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _ownerClient.GetAsync($"/api/resources/{id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Moderated", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task UpdateResource_WithUnknownId_ReturnsNotFound()
    {
        var response = await _ownerClient.PutAsJsonAsync($"/api/resources/{Guid.NewGuid()}", ValidResourceBody());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchResource_AsOwner_AppliesPartialUpdate()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();
        var patch = new[] { new { op = "replace", path = "/description", value = "Patched description." } };
        var content = new StringContent(JsonSerializer.Serialize(patch), Encoding.UTF8, "application/json-patch+json");

        var response = await _ownerClient.PatchAsync($"/api/resources/{id}", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _ownerClient.GetAsync($"/api/resources/{id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Patched description.", body.GetProperty("description").GetString());
        Assert.Equal("Syllabus", body.GetProperty("name").GetString());
    }

    // --- Delete ---

    [Fact]
    public async Task RemoveResource_AsOwner_DeletesResource()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _ownerClient.DeleteAsync($"/api/resources/{id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _ownerClient.GetAsync($"/api/resources/{id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task RemoveResource_AsNonOwnerNonAdmin_ReturnsForbidden()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _otherClient.DeleteAsync($"/api/resources/{id}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RemoveResource_AsAdmin_CanDeleteAnyResource()
    {
        var created = await CreateResourceAsync(_ownerClient);
        var id = created.GetProperty("id").GetString();

        var response = await _adminClient.DeleteAsync($"/api/resources/{id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
