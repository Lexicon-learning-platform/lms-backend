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
        ActionResponse Register(RegisterDto model, string role);

        ActionResponse DisableUser(string userId);

        ActionResponse DeleteUser(string userId);

        ActionResponse UpdateUser(string userId, UpdateUserDto model);
        ActionResponse ResetPassword(string userId, string newPassword);

        ActionResponse AddCourseToUser(string userId, string courseId);

        ActionResponse RemoveCourseFromUser(string userId, string courseId);

        ActionResponse GetUserStatistics(string userId);

        List<ApplicationUser> GetAllUsers();


        //Get statistics?

    }
}
