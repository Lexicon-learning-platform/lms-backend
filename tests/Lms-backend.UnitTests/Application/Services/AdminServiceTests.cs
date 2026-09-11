using Lms_backend.Application.Models;
using Lms_backend.Application.Services;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.UnitTests.TestHelpers;

namespace Lms_backend.UnitTests.Application.Services;

public class AdminServiceTests
{
    private static ApplicationUser NewUser(Guid? id = null, string userName = "existing.user", string? passwordHash = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        UserName = userName,
        GivenName = "Existing",
        LastName = "User",
        Email = "existing.user@example.com",
        Role = "Student",
        PasswordHash = passwordHash,
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        UpdatedAt = new DateTime(2026, 1, 2, 0, 0, 0, DateTimeKind.Utc),
    };

    private static RegisterDto ValidRegisterDto() => new()
    {
        Username = "new.user",
        Password = "P@ssw0rd!",
    };

    private static (FakeUserManager, FakeRoleManager, FakeCourseRepository, AdminService) CreateService()
    {
        var userManager = FakeUserManager.Create();
        var roleManager = FakeRoleManager.Create();
        var courseRepository = new FakeCourseRepository();
        var service = new AdminService(userManager, roleManager, courseRepository);
        return (userManager, roleManager, courseRepository, service);
    }

    // --- DeleteUser ---

    [Fact]
    public void DeleteUser_DeletesUserAndReturnsSuccess_WhenUserExists()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var result = service.DeleteUser(user.Id.ToString());

        Assert.Equal(ActionResponse.Success, result);
        Assert.Equal([user], userManager.DeletedUsers);
    }

    [Fact]
    public void DeleteUser_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.DeleteUser(Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.UserNotFound, result);
    }

    // --- DisableUser ---

    [Fact]
    public void DisableUser_SetsLockoutEndDateToMaxValue_WhenUserExists()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var result = service.DisableUser(user.Id.ToString());

        Assert.Equal(ActionResponse.Success, result);
        var call = Assert.Single(userManager.SetLockoutEndDateCalls);
        Assert.Equal(user, call.User);
        Assert.Equal(DateTimeOffset.MaxValue, call.LockoutEnd);
    }

    [Fact]
    public void DisableUser_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.DisableUser(Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.UserNotFound, result);
    }

    // --- EnableUser ---

    [Fact]
    public void EnableUser_SetsLockoutEndDateToNull_WhenUserExists()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var result = service.EnableUser(user.Id.ToString());

        Assert.Equal(ActionResponse.Success, result);
        var call = Assert.Single(userManager.SetLockoutEndDateCalls);
        Assert.Equal(user, call.User);
        Assert.Null(call.LockoutEnd);
    }

    [Fact]
    public void EnableUser_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.EnableUser(Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.UserNotFound, result);
    }

    // --- GetAllUsers ---

    [Fact]
    public void GetAllUsers_ReturnsAllUsersFromUserManager()
    {
        var (userManager, _, _, service) = CreateService();
        var first = NewUser(userName: "a.user");
        var second = NewUser(userName: "b.user");
        userManager.UsersList.AddRange([first, second]);

        var result = service.GetAllUsers();

        Assert.Equal([first, second], result);
    }

    // --- GetUserStatistics ---

    [Fact]
    public void GetUserStatistics_ReturnsMappedStatsAndSuccess_WhenUserExists()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var (response, statistics) = service.GetUserStatistics(user.Id.ToString());

        Assert.Equal(ActionResponse.Success, response);
        Assert.NotNull(statistics);
        Assert.Equal(user.Id, statistics!.Id);
        Assert.Equal(user.CreatedAt, statistics.CreatedAt);
        Assert.Equal(user.UpdatedAt, statistics.UpdatedAt);
        Assert.Equal(user.UserName, statistics.UserName);
        Assert.Equal(user.GivenName, statistics.GivenName);
        Assert.Equal(user.LastName, statistics.LastName);
        Assert.Empty(statistics.Courses);
    }

    [Fact]
    public void GetUserStatistics_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var (response, statistics) = service.GetUserStatistics(Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.UserNotFound, response);
        Assert.Null(statistics);
    }

    // --- Register ---

    [Fact]
    public void Register_CreatesUserWithGivenRole_WhenUsernameIsAvailableAndRoleExists()
    {
        var (userManager, roleManager, _, service) = CreateService();
        roleManager.RolesList.Add(new ApplicationRole { Name = "Teacher" });
        var data = ValidRegisterDto();

        var result = service.Register(data, "Teacher");

        Assert.Equal(ActionResponse.Success, result);
        var created = Assert.Single(userManager.CreatedUsers);
        Assert.Equal(data.Username, created.UserName);
        Assert.Equal("Teacher", created.Role);
        var passwordCall = Assert.Single(userManager.AddPasswordCalls);
        Assert.Equal(created, passwordCall.User);
        Assert.Equal(data.Password, passwordCall.Password);
    }

    [Fact]
    public void Register_ReturnsUserAlreadyExists_WhenUsernameIsTaken()
    {
        var (userManager, roleManager, _, service) = CreateService();
        roleManager.RolesList.Add(new ApplicationRole { Name = "Teacher" });
        var data = ValidRegisterDto();
        userManager.UsersList.Add(NewUser(userName: data.Username));

        var result = service.Register(data, "Teacher");

        Assert.Equal(ActionResponse.UserAlreadyExists, result);
        Assert.Empty(userManager.CreatedUsers);
    }

    [Fact]
    public void Register_ReturnsInvalidRole_WhenRoleDoesNotExist()
    {
        var (userManager, _, _, service) = CreateService();
        var data = ValidRegisterDto();

        var result = service.Register(data, "NonExistentRole");

        Assert.Equal(ActionResponse.InvalidRole, result);
        Assert.Empty(userManager.CreatedUsers);
    }

    // --- AddCourseToUser ---

    [Fact]
    public void AddCourseToUser_SetsCourseAndReturnsSuccess_WhenUserAndCourseExist()
    {
        var (userManager, _, courseRepository, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);
        var course = new Course { Id = Guid.NewGuid(), Name = "Course", Description = "Description", StartDate = DateOnly.FromDateTime(DateTime.UtcNow), Duration = 10 };
        courseRepository.ReadOnlyCourses.Add(course);

        var result = service.AddCourseToUser(user.Id.ToString(), course.Id.ToString());

        Assert.Equal(ActionResponse.Success, result);
        Assert.Equal(course.Id, user.CourseId);
        Assert.Equal(course, user.Course);
        Assert.Equal([user], userManager.UpdatedUsers);
    }

    [Fact]
    public void AddCourseToUser_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.AddCourseToUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.UserNotFound, result);
    }

    [Fact]
    public void AddCourseToUser_ReturnsBadData_WhenCourseIdIsNotAGuid()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var result = service.AddCourseToUser(user.Id.ToString(), "not-a-guid");

        Assert.Equal(ActionResponse.BadData, result);
    }

    [Fact]
    public void AddCourseToUser_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var result = service.AddCourseToUser(user.Id.ToString(), Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.NotFound, result);
    }

    // --- RemoveCourseFromUser ---

    [Fact]
    public void RemoveCourseFromUser_ClearsCourseAndReturnsSuccess_WhenUserAndCourseExist()
    {
        var (userManager, _, courseRepository, service) = CreateService();
        var course = new Course { Id = Guid.NewGuid(), Name = "Course", Description = "Description", StartDate = DateOnly.FromDateTime(DateTime.UtcNow), Duration = 10 };
        var user = NewUser();
        user.Course = course;
        user.CourseId = course.Id;
        userManager.UsersList.Add(user);
        courseRepository.ReadOnlyCourses.Add(course);

        var result = service.RemoveCourseFromUser(user.Id.ToString(), course.Id.ToString());

        Assert.Equal(ActionResponse.Success, result);
        Assert.Null(user.CourseId);
        Assert.Null(user.Course);
        Assert.Equal([user], userManager.UpdatedUsers);
    }

    [Fact]
    public void RemoveCourseFromUser_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.RemoveCourseFromUser(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.UserNotFound, result);
    }

    [Fact]
    public void RemoveCourseFromUser_ReturnsNotFound_WhenCourseDoesNotExist()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);

        var result = service.RemoveCourseFromUser(user.Id.ToString(), Guid.NewGuid().ToString());

        Assert.Equal(ActionResponse.NotFound, result);
    }

    // --- ResetPassword ---

    [Fact]
    public void ResetPassword_ChangesPassword_WhenUserHasExistingPasswordHash()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser(passwordHash: "existing-hash");
        userManager.UsersList.Add(user);

        var result = service.ResetPassword(user.Id.ToString(), "NewP@ssw0rd!");

        Assert.Equal(ActionResponse.Success, result);
        var call = Assert.Single(userManager.ChangePasswordCalls);
        Assert.Equal(user, call.User);
        Assert.Equal("existing-hash", call.CurrentPassword);
        Assert.Equal("NewP@ssw0rd!", call.NewPassword);
        Assert.Empty(userManager.AddPasswordCalls);
    }

    [Fact]
    public void ResetPassword_AddsPassword_WhenUserHasNoPasswordHash()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser(passwordHash: null);
        userManager.UsersList.Add(user);

        var result = service.ResetPassword(user.Id.ToString(), "NewP@ssw0rd!");

        Assert.Equal(ActionResponse.Success, result);
        var call = Assert.Single(userManager.AddPasswordCalls);
        Assert.Equal(user, call.User);
        Assert.Equal("NewP@ssw0rd!", call.Password);
        Assert.Empty(userManager.ChangePasswordCalls);
    }

    [Fact]
    public void ResetPassword_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.ResetPassword(Guid.NewGuid().ToString(), "NewP@ssw0rd!");

        Assert.Equal(ActionResponse.UserNotFound, result);
    }

    // --- UpdateUser ---

    [Fact]
    public void UpdateUser_UpdatesProvidedFieldsAndReturnsSuccess()
    {
        var (userManager, roleManager, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);
        roleManager.RolesList.Add(new ApplicationRole { Name = "Teacher" });
        var model = new UpdateUserDto
        {
            Username = "updated.user",
            GivenName = "Updated",
            LastName = "Name",
            Email = "updated@example.com",
            Role = "Teacher",
        };

        var result = service.UpdateUser(user.Id.ToString(), model);

        Assert.Equal(ActionResponse.Success, result);
        Assert.Equal("updated.user", user.UserName);
        Assert.Equal("Updated", user.GivenName);
        Assert.Equal("Name", user.LastName);
        Assert.Equal("updated@example.com", user.Email);
        Assert.Equal("Teacher", user.Role);
        Assert.Equal([user], userManager.UpdatedUsers);
    }

    [Fact]
    public void UpdateUser_KeepsExistingValues_WhenFieldsAreNull()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);
        var originalUserName = user.UserName;
        var originalRole = user.Role;
        var model = new UpdateUserDto();

        var result = service.UpdateUser(user.Id.ToString(), model);

        Assert.Equal(ActionResponse.Success, result);
        Assert.Equal(originalUserName, user.UserName);
        Assert.Equal(originalRole, user.Role);
    }

    [Fact]
    public void UpdateUser_ReturnsBadData_WhenModelIsNull()
    {
        var (_, _, _, service) = CreateService();

        var result = service.UpdateUser(Guid.NewGuid().ToString(), null!);

        Assert.Equal(ActionResponse.BadData, result);
    }

    [Fact]
    public void UpdateUser_ReturnsInvalidRole_WhenRoleDoesNotExist()
    {
        var (userManager, _, _, service) = CreateService();
        var user = NewUser();
        userManager.UsersList.Add(user);
        var model = new UpdateUserDto { Role = "NonExistentRole" };

        var result = service.UpdateUser(user.Id.ToString(), model);

        Assert.Equal(ActionResponse.InvalidRole, result);
    }

    [Fact]
    public void UpdateUser_ReturnsUserNotFound_WhenUserDoesNotExist()
    {
        var (_, _, _, service) = CreateService();

        var result = service.UpdateUser(Guid.NewGuid().ToString(), new UpdateUserDto());

        Assert.Equal(ActionResponse.UserNotFound, result);
    }
}
