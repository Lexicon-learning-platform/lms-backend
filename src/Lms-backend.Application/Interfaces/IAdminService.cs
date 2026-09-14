using Lms_backend.Domain.Entities;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Application.Interfaces
{
    public interface IAdminService
    {
        Task<ActionResponse> Register(RegisterDto model, string role);

        Task<ActionResponse> DisableUser(string userId);
        Task<ActionResponse> EnableUser(string userId);

        Task<ActionResponse> DeleteUser(string userId);

        Task<ActionResponse> UpdateUser(string userId, UpdateUserDto model);
        Task<ActionResponse> ResetPassword(string userId, string newPassword);

        Task<ActionResponse> AddCourseToUser(string userId, string courseId);

        Task<ActionResponse> RemoveCourseFromUser(string userId, string courseId);

        Task<(ActionResponse response, UserStatsDto? statistics)> GetUserStatistics(string userId);

        List<ApplicationUser> GetAllUsers();


        //Get statistics?

    }
}
