using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure;
using Lms_backend.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lms_backend.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class ActivitiesControllerTests(IntegrationTestWebAppFactory factory) : IAsyncLifetime
{
    private HttpClient _anonymousClient = null!;
    private HttpClient _teacherClient = null!;
    private HttpClient _studentClient = null!;

    public async Task InitializeAsync()
    {
        await factory.ResetDatabaseAsync();
        _anonymousClient = factory.CreateAuthClient();
        _teacherClient = await TestAuth.CreateAuthenticatedClientAsync(factory, "alex.nilsson", UserSeeder.DefaultPassword);
        _studentClient = await TestAuth.CreateAuthenticatedClientAsync(factory, "maria.svensson", UserSeeder.DefaultPassword);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static object ValidActivityBody(string name = "Kickoff Lecture", int startOffset = 0, int duration = 60, int type = 0) => new
    {
        name,
        description = "An activity created for integration tests.",
        startOffset,
        duration,
        type,
    };

    private async Task<Module> CreateModuleDirectlyAsync(string name = "Existing Module")
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var module = new Module
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = "Seeded directly for an ActivitiesController integration test.",
            Duration = 3,
        };
        db.Modules.Add(module);
        await db.SaveChangesAsync();
        return module;
    }

    // --- Create ---

    [Fact]
    public async Task CreateActivity_AsTeacher_ReturnsCreatedWithLocation()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Kickoff Lecture", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task CreateActivity_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _studentClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateActivity_WithoutToken_ReturnsUnauthorized()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _anonymousClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateActivity_WithUnknownModuleId_ReturnsNotFound()
    {
        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{Guid.NewGuid()}/activities", ValidActivityBody());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateActivity_WithTooShortName_ReturnsBadRequest()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(name: "ab"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateActivity_WithNegativeStartOffset_ReturnsBadRequest()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(startOffset: -1));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateActivity_OverlappingSameTypeActivity_ReturnsBadRequest()
    {
        var module = await CreateModuleDirectlyAsync();
        await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(startOffset: 0, duration: 60, type: 0));

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(name: "Overlapping Lecture", startOffset: 30, duration: 60, type: 0));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateActivity_OverlappingDifferentTypeActivity_ReturnsCreated()
    {
        var module = await CreateModuleDirectlyAsync();
        await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(startOffset: 0, duration: 60, type: 0));

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(name: "Overlapping Exercise", startOffset: 30, duration: 60, type: 2));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    // --- Get ---

    [Fact]
    public async Task GetActivity_AsAuthenticatedUser_ReturnsActivity()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Kickoff Lecture", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetActivity_WithoutToken_ReturnsUnauthorized()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _anonymousClient.GetAsync($"/api/modules/{module.Id}/activities/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetActivity_WithUnknownId_ReturnsNotFound()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetActivity_BelongingToDifferentModule_ReturnsNotFound()
    {
        var module = await CreateModuleDirectlyAsync();
        var otherModule = await CreateModuleDirectlyAsync(name: "Other Module");
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _studentClient.GetAsync($"/api/modules/{otherModule.Id}/activities/{activityId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetActivities_WithoutToken_ReturnsUnauthorized()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _anonymousClient.GetAsync($"/api/modules/{module.Id}/activities");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetActivities_FiltersByType()
    {
        var module = await CreateModuleDirectlyAsync();
        await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(name: "A Lecture", type: 0));
        await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody(name: "An Exercise", startOffset: 120, type: 2));

        var response = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities?type=Exercise");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var activities = body.EnumerateArray().ToList();
        Assert.Single(activities);
        Assert.Equal("An Exercise", activities[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetActivities_IncludesPaginationHeader()
    {
        var module = await CreateModuleDirectlyAsync();
        await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());

        var response = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities");

        Assert.True(response.Headers.TryGetValues("X-Pagination", out _));
    }

    // --- Update ---

    [Fact]
    public async Task UpdateActivity_AsTeacher_PersistsChanges()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _teacherClient.PutAsJsonAsync($"/api/modules/{module.Id}/activities/{activityId}", ValidActivityBody(name: "Renamed Activity", duration: 90));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Renamed Activity", body.GetProperty("name").GetString());
        Assert.Equal(90, body.GetProperty("duration").GetInt32());
    }

    [Fact]
    public async Task UpdateActivity_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _studentClient.PutAsJsonAsync($"/api/modules/{module.Id}/activities/{activityId}", ValidActivityBody());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateActivity_WithUnknownId_ReturnsNotFound()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.PutAsJsonAsync($"/api/modules/{module.Id}/activities/{Guid.NewGuid()}", ValidActivityBody());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchActivity_AsTeacher_AppliesPartialUpdate()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();
        var patch = new[] { new { op = "replace", path = "/duration", value = 45 } };
        var content = new StringContent(JsonSerializer.Serialize(patch), Encoding.UTF8, "application/json-patch+json");

        var response = await _teacherClient.PatchAsync($"/api/modules/{module.Id}/activities/{activityId}", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(45, body.GetProperty("duration").GetInt32());
        Assert.Equal("Kickoff Lecture", body.GetProperty("name").GetString());
    }

    // --- Delete ---

    [Fact]
    public async Task RemoveActivity_AsTeacher_DeletesActivity()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _teacherClient.DeleteAsync($"/api/modules/{module.Id}/activities/{activityId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task RemoveActivity_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _studentClient.DeleteAsync($"/api/modules/{module.Id}/activities/{activityId}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // --- Resources ---

    [Fact]
    public async Task CreateActivityResource_AsTeacher_ReturnsCreatedAndIsListed()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();
        var resourceBody = new { name = "Activity Handout", description = "Handout for this activity.", type = 1, data = "https://example.com/handout.pdf" };

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities/{activityId}/resources", resourceBody);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var listResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}/resources");
        var resources = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Single(resources.EnumerateArray());
    }

    [Fact]
    public async Task CreateActivityResource_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();
        var resourceBody = new { name = "Activity Handout", description = "Handout for this activity.", type = 1, data = "https://example.com/handout.pdf" };

        var response = await _studentClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities/{activityId}/resources", resourceBody);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetActivityResources_WithoutToken_ReturnsUnauthorized()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();

        var response = await _anonymousClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}/resources");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveActivityResource_AsTeacher_DetachesResource()
    {
        var module = await CreateModuleDirectlyAsync();
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities", ValidActivityBody());
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var activityId = created.GetProperty("id").GetString();
        var resourceBody = new { name = "Activity Handout", description = "Handout for this activity.", type = 1, data = "https://example.com/handout.pdf" };
        var createResourceResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/activities/{activityId}/resources", resourceBody);
        var createdResource = await createResourceResponse.Content.ReadFromJsonAsync<JsonElement>();
        var resourceId = createdResource.GetProperty("id").GetString();

        var response = await _teacherClient.DeleteAsync($"/api/modules/{module.Id}/activities/{activityId}/resources/{resourceId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var listResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/activities/{activityId}/resources");
        var resources = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Empty(resources.EnumerateArray());
    }
}
