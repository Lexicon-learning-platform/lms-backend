using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure;
using Lms_backend.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Lms_backend.IntegrationTests.Controllers;

[Collection(IntegrationTestCollection.Name)]
public class AdminControllerTests(IntegrationTestWebAppFactory factory) : IAsyncLifetime
{
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        await factory.ResetDatabaseAsync();
        _client = await TestAuth.CreateAuthenticatedClientAsync(factory, "admin", UserSeeder.DefaultPassword);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    private static (Guid Id, string UserName) SeededUser(string userName) =>
        TestUsers.Seeded.Where(u => u.UserName == userName).Select(u => (u.Id, u.UserName)).Single();

    private async Task<Course> CreateCourseAsync()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var course = new Course
        {
            Id = Guid.NewGuid(),
            Name = "Integration Test Course",
            Description = "Created directly for AdminController integration tests.",
            StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Duration = 4,
        };
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return course;
    }

    private async Task<ApplicationUser> ReloadUserAsync(Guid userId)
    {
        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return (await userManager.FindByIdAsync(userId.ToString()))!;
    }

    // --- Authorization ---

    [Fact]
    public async Task GetAllUsers_WithoutToken_ReturnsUnauthorized()
    {
        using var anonymousClient = factory.CreateAuthClient();

        var response = await anonymousClient.GetAsync("/api/admin/getusers");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAllUsers_WithNonAdminToken_ReturnsForbidden()
    {
        using var studentClient = await TestAuth.CreateAuthenticatedClientAsync(factory, "maria.svensson", UserSeeder.DefaultPassword);

        var response = await studentClient.GetAsync("/api/admin/getusers");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // --- Register ---

    [Fact]
    public async Task Register_WithNewUsernameAndValidRole_CreatesUserWithThatRole()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/register?role=Teacher", new { username = "new.teacher", password = "P@ssw0rd!" });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var users = await _client.GetFromJsonAsync<JsonElement>("/api/admin/getusers");
        var created = users.EnumerateArray().Single(u => u.GetProperty("userName").GetString() == "new.teacher");
        Assert.Equal("Teacher", created.GetProperty("role").GetString());
    }

    [Fact]
    public async Task Register_WithExistingUsername_ReturnsConflict()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/register?role=Teacher", new { username = "maria.svensson", password = "P@ssw0rd!" });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithInvalidRole_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/admin/register?role=NotARole", new { username = "new.person", password = "P@ssw0rd!" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // --- GetAllUsers ---

    [Fact]
    public async Task GetAllUsers_IncludesSeededUsers()
    {
        var users = await _client.GetFromJsonAsync<JsonElement>("/api/admin/getusers");

        Assert.Contains(users.EnumerateArray(), u => u.GetProperty("userName").GetString() == "maria.svensson");
    }

    // --- DeleteUser ---

    [Fact]
    public async Task DeleteUser_RemovesUserPermanently()
    {
        await _client.PostAsJsonAsync("/api/admin/register?role=Student", new { username = "throwaway.user", password = "P@ssw0rd!" });
        var usersBefore = await _client.GetFromJsonAsync<JsonElement>("/api/admin/getusers");
        var userId = usersBefore.EnumerateArray().Single(u => u.GetProperty("userName").GetString() == "throwaway.user").GetProperty("id").GetString();

        var response = await _client.DeleteAsync($"/api/admin/deleteuser/{userId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var usersAfter = await _client.GetFromJsonAsync<JsonElement>("/api/admin/getusers");
        Assert.DoesNotContain(usersAfter.EnumerateArray(), u => u.GetProperty("userName").GetString() == "throwaway.user");
    }

    [Fact]
    public async Task DeleteUser_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync($"/api/admin/deleteuser/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // --- DisableUser / EnableUser ---

    [Fact]
    public async Task DisableUser_PersistsLockout()
    {
        var (userId, _) = SeededUser("johan.berg");

        var response = await _client.PutAsync($"/api/admin/disableuser/{userId}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await ReloadUserAsync(userId);
        Assert.NotNull(user.LockoutEnd);
        Assert.True(user.LockoutEnd > DateTimeOffset.UtcNow.AddYears(1));
    }

    [Fact]
    public async Task EnableUser_ClearsLockout()
    {
        var (userId, _) = SeededUser("johan.berg");
        await _client.PutAsync($"/api/admin/disableuser/{userId}", null);

        var response = await _client.PutAsync($"/api/admin/enableuser/{userId}", null);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await ReloadUserAsync(userId);
        Assert.Null(user.LockoutEnd);
    }

    [Fact]
    public async Task DisableUser_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.PutAsync($"/api/admin/disableuser/{Guid.NewGuid()}", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // --- GetUserStatistics ---

    [Fact]
    public async Task GetUserStatistics_ReturnsStatsForExistingUser()
    {
        var (userId, userName) = SeededUser("johan.berg");

        var response = await _client.GetAsync($"/api/admin/getuserstats/{userId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var stats = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(userName, stats.GetProperty("userName").GetString());
    }

    [Fact]
    public async Task GetUserStatistics_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.GetAsync($"/api/admin/getuserstats/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // --- UpdateUser ---

    [Fact]
    public async Task UpdateUser_PersistsProvidedFields()
    {
        var (userId, _) = SeededUser("johan.berg");

        var response = await _client.PutAsJsonAsync($"/api/admin/updateuser/{userId}", new { givenName = "Updated", role = "Teacher" });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var user = await ReloadUserAsync(userId);
        Assert.Equal("Updated", user.GivenName);
        Assert.Equal("Teacher", user.Role);
    }

    [Fact]
    public async Task UpdateUser_WithInvalidRole_ReturnsBadRequest()
    {
        var (userId, _) = SeededUser("johan.berg");

        var response = await _client.PutAsJsonAsync($"/api/admin/updateuser/{userId}", new { role = "NotARole" });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync($"/api/admin/updateuser/{Guid.NewGuid()}", new { givenName = "Nobody" });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // --- ResetPassword ---

    [Fact]
    public async Task ResetPassword_AllowsLoginWithNewPassword()
    {
        var (userId, userName) = SeededUser("johan.berg");

        var response = await _client.PutAsJsonAsync($"/api/admin/resetpassword/{userId}", "BrandNewP@ssw0rd!");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var freshClient = factory.CreateAuthClient();
        var loginResponse = await freshClient.PostAsJsonAsync("/api/auth/login", new { username = userName, password = "BrandNewP@ssw0rd!" });
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
    }

    [Fact]
    public async Task ResetPassword_WithUnknownId_ReturnsNotFound()
    {
        var response = await _client.PutAsJsonAsync($"/api/admin/resetpassword/{Guid.NewGuid()}", "BrandNewP@ssw0rd!");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    // --- AddCourseToUser / RemoveCourseFromUser ---

    [Fact]
    public async Task AddCourseToUser_ThenRemoveCourseFromUser_PersistsBothChanges()
    {
        var (userId, _) = SeededUser("johan.berg");
        var course = await CreateCourseAsync();

        var addResponse = await _client.PutAsync($"/api/admin/addcoursetouser/{userId}/{course.Id}", null);
        Assert.Equal(HttpStatusCode.OK, addResponse.StatusCode);

        var userAfterAdd = await ReloadUserAsync(userId);
        Assert.Equal(course.Id, userAfterAdd.CourseId);

        var removeResponse = await _client.PutAsync($"/api/admin/removecoursefromuser/{userId}/{course.Id}", null);
        Assert.Equal(HttpStatusCode.OK, removeResponse.StatusCode);

        var userAfterRemove = await ReloadUserAsync(userId);
        Assert.Null(userAfterRemove.CourseId);
    }

    [Fact]
    public async Task AddCourseToUser_WithNonGuidCourseId_ReturnsBadRequest()
    {
        var (userId, _) = SeededUser("johan.berg");

        var response = await _client.PutAsync($"/api/admin/addcoursetouser/{userId}/not-a-guid", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AddCourseToUser_WithUnknownCourse_ReturnsNotFound()
    {
        var (userId, _) = SeededUser("johan.berg");

        var response = await _client.PutAsync($"/api/admin/addcoursetouser/{userId}/{Guid.NewGuid()}", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AddCourseToUser_WithUnknownUser_ReturnsNotFound()
    {
        var course = await CreateCourseAsync();

        var response = await _client.PutAsync($"/api/admin/addcoursetouser/{Guid.NewGuid()}/{course.Id}", null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
