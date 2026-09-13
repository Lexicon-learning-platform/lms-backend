using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure;
using Lms_backend.IntegrationTests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lms_backend.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class ModulesControllerTests(IntegrationTestWebAppFactory factory) : IAsyncLifetime
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

    private static object ValidModuleBody(string name = "Introduction to REST APIs", int duration = 5) => new
    {
        name,
        description = "A module created for integration tests.",
        duration,
    };

    private async Task<Module> CreateModuleDirectlyAsync(string name = "Existing Module")
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var module = new Module
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = "Seeded directly for a ModulesController integration test.",
            Duration = 3,
        };
        db.Modules.Add(module);
        await db.SaveChangesAsync();
        return module;
    }

    private async Task<Course> CreateCourseWithModuleAsync(Module module)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Course For Module Filter",
            Description = "Seeded directly for a ModulesController integration test.",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
            Duration = 8,
        };
        db.Courses.Add(course);
        db.CourseModules.Add(new CourseModule { CourseId = course.Id, ModuleId = module.Id, StartTimeOffset = 0 });
        await db.SaveChangesAsync();
        return course;
    }

    // --- Create ---

    [Fact]
    public async Task CreateModule_AsTeacher_ReturnsCreatedWithLocation()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/modules", ValidModuleBody());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Introduction to REST APIs", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task CreateModule_AsStudent_ReturnsForbidden()
    {
        var response = await _studentClient.PostAsJsonAsync("/api/modules", ValidModuleBody());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateModule_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.PostAsJsonAsync("/api/modules", ValidModuleBody());

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateModule_WithTooShortName_ReturnsBadRequest()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/modules", ValidModuleBody(name: "ab"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateModule_WithNegativeDuration_ReturnsBadRequest()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/modules", ValidModuleBody(duration: -1));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // --- Get ---

    [Fact]
    public async Task GetModule_AsAuthenticatedUser_ReturnsModule()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _studentClient.GetAsync($"/api/modules/{module.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(module.Name, body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetModule_WithoutToken_ReturnsUnauthorized()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _anonymousClient.GetAsync($"/api/modules/{module.Id}");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetModule_WithUnknownId_ReturnsNotFound()
    {
        var response = await _studentClient.GetAsync($"/api/modules/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetModules_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.GetAsync("/api/modules");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetModules_FiltersByName()
    {
        await CreateModuleDirectlyAsync(name: "Advanced Kotlin");
        await CreateModuleDirectlyAsync(name: "Beginner Python");

        var response = await _studentClient.GetAsync("/api/modules?name=Kotlin");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var modules = body.EnumerateArray().ToList();
        Assert.Single(modules);
        Assert.Equal("Advanced Kotlin", modules[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetModules_FiltersByCourse()
    {
        var attachedModule = await CreateModuleDirectlyAsync(name: "Attached Module");
        await CreateModuleDirectlyAsync(name: "Unattached Module");
        var course = await CreateCourseWithModuleAsync(attachedModule);

        var response = await _studentClient.GetAsync($"/api/modules?course={course.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var modules = body.EnumerateArray().ToList();
        Assert.Single(modules);
        Assert.Equal("Attached Module", modules[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetModules_IncludesPaginationHeader()
    {
        await CreateModuleDirectlyAsync();

        var response = await _studentClient.GetAsync("/api/modules");

        Assert.True(response.Headers.TryGetValues("X-Pagination", out _));
    }

    // --- Update ---

    [Fact]
    public async Task UpdateModule_AsTeacher_PersistsChanges()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.PutAsJsonAsync($"/api/modules/{module.Id}", ValidModuleBody(name: "Renamed Module"));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Renamed Module", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task UpdateModule_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _studentClient.PutAsJsonAsync($"/api/modules/{module.Id}", ValidModuleBody());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateModule_WithUnknownId_ReturnsNotFound()
    {
        var response = await _teacherClient.PutAsJsonAsync($"/api/modules/{Guid.NewGuid()}", ValidModuleBody());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchModule_AsTeacher_AppliesPartialUpdate()
    {
        var module = await CreateModuleDirectlyAsync();
        var patch = new[] { new { op = "replace", path = "/duration", value = 7 } };
        var content = new StringContent(JsonSerializer.Serialize(patch), Encoding.UTF8, "application/json-patch+json");

        var response = await _teacherClient.PatchAsync($"/api/modules/{module.Id}", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(7, body.GetProperty("duration").GetInt32());
        Assert.Equal(module.Name, body.GetProperty("name").GetString());
    }

    // --- Delete ---

    [Fact]
    public async Task RemoveModule_AsTeacher_DeletesModule()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.DeleteAsync($"/api/modules/{module.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task RemoveModule_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _studentClient.DeleteAsync($"/api/modules/{module.Id}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // --- Resources ---

    [Fact]
    public async Task CreateModuleResource_AsTeacher_ReturnsCreatedAndIsListed()
    {
        var module = await CreateModuleDirectlyAsync();
        var resourceBody = new { name = "Lecture Slides", description = "Slides for this module.", type = 1, data = "https://example.com/slides.pdf" };

        var response = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/resources", resourceBody);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var listResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/resources");
        var resources = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Single(resources.EnumerateArray());
    }

    [Fact]
    public async Task CreateModuleResource_AsStudent_ReturnsForbidden()
    {
        var module = await CreateModuleDirectlyAsync();
        var resourceBody = new { name = "Lecture Slides", description = "Slides for this module.", type = 1, data = "https://example.com/slides.pdf" };

        var response = await _studentClient.PostAsJsonAsync($"/api/modules/{module.Id}/resources", resourceBody);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetModuleResources_WithoutToken_ReturnsUnauthorized()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _anonymousClient.GetAsync($"/api/modules/{module.Id}/resources");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveModuleResource_AsTeacher_DetachesResource()
    {
        var module = await CreateModuleDirectlyAsync();
        var resourceBody = new { name = "Lecture Slides", description = "Slides for this module.", type = 1, data = "https://example.com/slides.pdf" };
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/modules/{module.Id}/resources", resourceBody);
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var resourceId = created.GetProperty("id").GetString();

        var response = await _teacherClient.DeleteAsync($"/api/modules/{module.Id}/resources/{resourceId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var listResponse = await _studentClient.GetAsync($"/api/modules/{module.Id}/resources");
        var resources = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Empty(resources.EnumerateArray());
    }
}
