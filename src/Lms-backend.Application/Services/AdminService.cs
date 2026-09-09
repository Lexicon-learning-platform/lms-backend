using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Application.Services
{
    public class AdminService(UserManager<ApplicationUser> userManager, ICourseRepository courseRepository) : IAdminService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ICourseRepository _courseRepository = courseRepository;

        public ActionResponse DeleteUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return ActionResponse.UserNotFound;

            _userManager.DeleteAsync(user);
            return ActionResponse.Success;
        }

        public ActionResponse DisableUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
                return ActionResponse.UserNotFound;

            _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            return ActionResponse.Success;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return users;
        }

        public ActionResponse GetUserStatistics(string userId)
        {
            var user = GetUserById(userId);
            if (user == null) return ActionResponse.NotFound;

            UserStatsDto userStatsDto = new UserStatsDto()
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

            return ActionResponse.Success;
        }

        public ActionResponse Register(RegisterDto model, string role)
        {
            //Check if the user already exists
            var existingUser = _userManager.FindByNameAsync(model.Username).Result;
            if (existingUser != null)
                return ActionResponse.UserAlreadyExists;

            //Create a new user and store it in the database

            ApplicationUser newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = model.Username,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = role
            };

            _userManager.CreateAsync(newUser);

            _userManager.AddPasswordAsync(newUser, model.Password);

            return ActionResponse.Success;
        }
        public ActionResponse AddCourseToUser(string userId, string courseId)
        {
            var user = GetUserById(userId);
            if(user == null) return ActionResponse.NotFound;

            //TODO: Make sure to error check for illegitimate courseId
            //This assumes courseId is parsable to a Guid.

            var course = _courseRepository.GetCourseReadOnlyAsync(Guid.Parse(courseId), CancellationToken.None).Result;
            if (course == null) return ActionResponse.NotFound;

            user.Course = course;
            user.CourseId = course.Id;

            _userManager.UpdateAsync(user);
            return ActionResponse.Success;

        }

        public ActionResponse RemoveCourseFromUser(string userId, string courseId)
        {
            var user = GetUserById(userId);
            if (user == null) return ActionResponse.NotFound;

            var course = _courseRepository.GetCourseReadOnlyAsync(Guid.Parse(courseId), CancellationToken.None).Result;
            if (course == null) return ActionResponse.NotFound;

            user.Course = null;
            user.CourseId = null;

            _userManager.UpdateAsync(user);
            return ActionResponse.Success;
        }

        public ActionResponse ResetPassword(string userId, string newPassword)
        {
            var user = GetUserById(userId);
            if (user == null) return ActionResponse.NotFound;

            var currentPassword = user.PasswordHash;

            if(currentPassword != null)
                _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            else
                _userManager.AddPasswordAsync(user, newPassword);

            return ActionResponse.Success;
        }

        public ActionResponse UpdateUser(string userId, UpdateUserDto model)
        {
            var user = GetUserById(userId);
            if (user == null) return ActionResponse.NotFound;

            user.UserName = model.Username ?? user.UserName;
            user.GivenName = model.GivenName ?? user.GivenName;
            user.LastName = model.LastName ?? user.LastName;
            user.Email = model.Email ?? user.Email;
            user.Role = model.Role ?? user.Role;

            _userManager.UpdateAsync(user);
            return ActionResponse.Success;
        }

        private ApplicationUser? GetUserById(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            return user;
        }

    }
}
