using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Lms_backend.Application.Services
{
    public class AdminService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ICourseRepository courseRepository) : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ICourseRepository _courseRepository = courseRepository;

        public async Task<ActionResponse> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ActionResponse.UserNotFound;

            await _userManager.DeleteAsync(user);
            return ActionResponse.Success;
        }

        public async Task<ActionResponse> DisableUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ActionResponse.UserNotFound;

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return ActionResponse.Success;
        }

        public async Task<ActionResponse> EnableUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ActionResponse.UserNotFound;
            await _userManager.SetLockoutEndDateAsync(user, null);
            return ActionResponse.Success;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return users;
        }

        public async Task<(ActionResponse response, UserStatsDto? statistics)> GetUserStatistics(string userId)
        {
            var user = await GetUserById(userId);
            if (user == null) return (ActionResponse.UserNotFound, null);

            UserStatsDto userStatsDto = new()
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                UserName = user.UserName ?? string.Empty,
                GivenName = user.GivenName ?? string.Empty,
                LastName = user.LastName ?? string.Empty,
                Courses = []
                //TODO: Populate the Courses property with the user's courses.
            };

            return (ActionResponse.Success, userStatsDto);
        }

        public async Task<ActionResponse> Register(RegisterDto model, string role)
        {
            //Check if the user already exists
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
                return ActionResponse.UserAlreadyExists;

            //Create a new user and store it in the database

            //Check if the role exists
            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                return ActionResponse.InvalidRole;

            ApplicationUser newUser = new()
            {
                Id = Guid.NewGuid(),
                UserName = model.Username,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = role
            };

            await _userManager.CreateAsync(newUser);

            await _userManager.AddPasswordAsync(newUser, model.Password);

            return ActionResponse.Success;
        }
        public async Task<ActionResponse> AddCourseToUser(string userId, string courseId)
        {
            var user = await GetUserById(userId);
            if(user == null) return ActionResponse.UserNotFound;

            var success = Guid.TryParse(courseId, out var parsedCourseId);
            if (!success) return ActionResponse.BadData;

            var course = await _courseRepository.GetCourseReadOnlyAsync(parsedCourseId, CancellationToken.None);
            if (course == null) return ActionResponse.NotFound;

            user.Course = course;
            user.CourseId = course.Id;

            await _userManager.UpdateAsync(user);
            return ActionResponse.Success;

        }

        public async Task<ActionResponse> RemoveCourseFromUser(string userId, string courseId)
        {
            var user = await GetUserById(userId);
            if (user == null) return ActionResponse.UserNotFound;

            var course = await _courseRepository.GetCourseReadOnlyAsync(Guid.Parse(courseId), CancellationToken.None);
            if (course == null) return ActionResponse.NotFound;

            user.Course = null;
            user.CourseId = null;

            await _userManager.UpdateAsync(user);
            return ActionResponse.Success;
        }

        public async Task<ActionResponse> ResetPassword(string userId, string newPassword)
        {
            var user = await GetUserById(userId);
            if (user == null) return ActionResponse.UserNotFound;

            if (await _userManager.HasPasswordAsync(user))
                await _userManager.RemovePasswordAsync(user);

            await _userManager.AddPasswordAsync(user, newPassword);

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> UpdateUser(string userId, UpdateUserDto model)
        {
            if(model == null) return ActionResponse.BadData;

            if(model.Role != null && await _roleManager.FindByNameAsync(model.Role) == null)
                return ActionResponse.InvalidRole;

            var user = await GetUserById(userId);
            if (user == null) return ActionResponse.UserNotFound;

            user.UserName = model.Username ?? user.UserName;
            user.GivenName = model.GivenName ?? user.GivenName;
            user.LastName = model.LastName ?? user.LastName;
            user.Email = model.Email ?? user.Email;
            user.Role = model.Role ?? user.Role;

            await _userManager.UpdateAsync(user);
            return ActionResponse.Success;
        }

        private async Task<ApplicationUser?> GetUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

    }
}
