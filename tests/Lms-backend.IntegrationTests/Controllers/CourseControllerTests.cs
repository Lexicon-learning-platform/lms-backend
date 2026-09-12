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
public class CourseControllerTests(IntegrationTestWebAppFactory factory) : IAsyncLifetime
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

    private static object ValidCourseBody(string name = "Full-Stack C#", DateOnly? startDate = null, int duration = 10, Guid[]? moduleIds = null) => new
    {
        name,
        description = "A course created for integration tests.",
        startDate = (startDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1))).ToString("yyyy-MM-dd"),
        duration,
        moduleIds = moduleIds ?? [],
    };

    private async Task<Course> CreateCourseDirectlyAsync(string name = "Existing Course", DateOnly? startDate = null)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = "Seeded directly for a CourseController integration test.",
            StartDate = startDate ?? DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
            Duration = 8,
        };
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return course;
    }

    private async Task<Module> CreateModuleDirectlyAsync(string name = "Existing Module")
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var module = new Module
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = "Seeded directly for a CourseController integration test.",
            Duration = 2,
        };
        db.Modules.Add(module);
        await db.SaveChangesAsync();
        return module;
    }

    // --- Create ---

    [Fact]
    public async Task CreateCourse_AsTeacher_ReturnsCreatedWithLocation()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/courses", ValidCourseBody());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Headers.Location);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Full-Stack C#", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task CreateCourse_AsStudent_ReturnsForbidden()
    {
        var response = await _studentClient.PostAsJsonAsync("/api/courses", ValidCourseBody());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.PostAsJsonAsync("/api/courses", ValidCourseBody());

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_WithStartDateInThePast_ReturnsBadRequest()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/courses", ValidCourseBody(startDate: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_WithTooShortName_ReturnsBadRequest()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/courses", ValidCourseBody(name: "ab"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_WithUnknownModuleId_ReturnsNotFound()
    {
        var response = await _teacherClient.PostAsJsonAsync("/api/courses", ValidCourseBody(moduleIds: [Guid.NewGuid()]));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateCourse_WithExistingModuleId_AttachesModule()
    {
        var module = await CreateModuleDirectlyAsync();

        var response = await _teacherClient.PostAsJsonAsync("/api/courses", ValidCourseBody(moduleIds: [module.Id]));

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var modules = body.GetProperty("modules").EnumerateArray().ToList();
        Assert.Single(modules);
        Assert.Equal(module.Id.ToString(), modules[0].GetProperty("id").GetString());
    }

    // --- Get ---

    [Fact]
    public async Task GetCourse_WithExistingId_ReturnsCourse_WithoutAuthentication()
    {
        var course = await CreateCourseDirectlyAsync();

        var response = await _anonymousClient.GetAsync($"/api/courses/{course.Id}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(course.Name, body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetCourse_WithUnknownId_ReturnsNotFound()
    {
        var response = await _anonymousClient.GetAsync($"/api/courses/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetCourses_FiltersByName()
    {
        await CreateCourseDirectlyAsync(name: "Advanced Kotlin");
        await CreateCourseDirectlyAsync(name: "Beginner Python");

        var response = await _anonymousClient.GetAsync("/api/courses?name=Kotlin");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<JsonElement>();
        var courses = body.EnumerateArray().ToList();
        Assert.Single(courses);
        Assert.Equal("Advanced Kotlin", courses[0].GetProperty("name").GetString());
    }

    [Fact]
    public async Task GetCourses_IncludesPaginationHeader()
    {
        await CreateCourseDirectlyAsync();

        var response = await _anonymousClient.GetAsync("/api/courses");

        Assert.True(response.Headers.TryGetValues("X-Pagination", out _));
    }

    [Fact]
    public async Task GetCurrentUserCourse_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _anonymousClient.GetAsync("/api/courses/my-course");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetCurrentUserCourse_ForUserWithNoCourse_ReturnsNoContent()
    {
        // Ok(null) is converted to 204 by the default HttpNoContentOutputFormatter.
        var response = await _studentClient.GetAsync("/api/courses/my-course");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    // --- Update ---

    [Fact]
    public async Task UpdateCourse_AsTeacher_PersistsChanges()
    {
        var course = await CreateCourseDirectlyAsync();

        var response = await _teacherClient.PutAsJsonAsync($"/api/courses/{course.Id}", ValidCourseBody(name: "Renamed Course", startDate: course.StartDate));

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _anonymousClient.GetAsync($"/api/courses/{course.Id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Renamed Course", body.GetProperty("name").GetString());
    }

    [Fact]
    public async Task UpdateCourse_AsStudent_ReturnsForbidden()
    {
        var course = await CreateCourseDirectlyAsync();

        var response = await _studentClient.PutAsJsonAsync($"/api/courses/{course.Id}", ValidCourseBody(startDate: course.StartDate));

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task UpdateCourse_WithUnknownId_ReturnsNotFound()
    {
        var response = await _teacherClient.PutAsJsonAsync($"/api/courses/{Guid.NewGuid()}", ValidCourseBody());

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PatchCourse_AsTeacher_AppliesPartialUpdate()
    {
        var course = await CreateCourseDirectlyAsync();
        var patch = new[] { new { op = "replace", path = "/description", value = "Patched description." } };
        var content = new StringContent(JsonSerializer.Serialize(patch), Encoding.UTF8, "application/json-patch+json");

        var response = await _teacherClient.PatchAsync($"/api/courses/{course.Id}", content);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        var getResponse = await _anonymousClient.GetAsync($"/api/courses/{course.Id}");
        var body = await getResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Patched description.", body.GetProperty("description").GetString());
        Assert.Equal(course.Name, body.GetProperty("name").GetString());
    }

    // --- Delete ---

    [Fact]
    public async Task RemoveCourse_AsTeacher_DeletesCourse()
    {
        var course = await CreateCourseDirectlyAsync();

        var response = await _teacherClient.DeleteAsync($"/api/courses/{course.Id}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var getResponse = await _anonymousClient.GetAsync($"/api/courses/{course.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task RemoveCourse_AsStudent_ReturnsForbidden()
    {
        var course = await CreateCourseDirectlyAsync();

        var response = await _studentClient.DeleteAsync($"/api/courses/{course.Id}");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // --- Resources ---

    [Fact]
    public async Task CreateCourseResource_AsTeacher_ReturnsCreatedAndIsListed()
    {
        var course = await CreateCourseDirectlyAsync();
        var resourceBody = new { name = "Syllabus", description = "Course syllabus document.", type = 1, data = "https://example.com/syllabus.pdf" };

        var response = await _teacherClient.PostAsJsonAsync($"/api/courses/{course.Id}/resources", resourceBody);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var listResponse = await _studentClient.GetAsync($"/api/courses/{course.Id}/resources");
        var resources = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Single(resources.EnumerateArray());
    }

    [Fact]
    public async Task CreateCourseResource_AsStudent_ReturnsForbidden()
    {
        var course = await CreateCourseDirectlyAsync();
        var resourceBody = new { name = "Syllabus", description = "Course syllabus document.", type = 1, data = "https://example.com/syllabus.pdf" };

        var response = await _studentClient.PostAsJsonAsync($"/api/courses/{course.Id}/resources", resourceBody);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetCourseResources_WithoutToken_ReturnsUnauthorized()
    {
        var course = await CreateCourseDirectlyAsync();

        var response = await _anonymousClient.GetAsync($"/api/courses/{course.Id}/resources");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveCourseResource_AsTeacher_DetachesResource()
    {
        var course = await CreateCourseDirectlyAsync();
        var resourceBody = new { name = "Syllabus", description = "Course syllabus document.", type = 1, data = "https://example.com/syllabus.pdf" };
        var createResponse = await _teacherClient.PostAsJsonAsync($"/api/courses/{course.Id}/resources", resourceBody);
        var created = await createResponse.Content.ReadFromJsonAsync<JsonElement>();
        var resourceId = created.GetProperty("id").GetString();

        var response = await _teacherClient.DeleteAsync($"/api/courses/{course.Id}/resources/{resourceId}");
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        var listResponse = await _studentClient.GetAsync($"/api/courses/{course.Id}/resources");
        var resources = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Empty(resources.EnumerateArray());
    }
}
