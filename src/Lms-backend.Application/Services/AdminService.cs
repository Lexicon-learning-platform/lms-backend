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
    public class AdminService(IAdminRepository adminRepository, IConfiguration configuration, UserManager<ApplicationUser> userManager) : IAdminService
    {
        private readonly IAdminRepository _repository = adminRepository;
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public ActionResponse AddCourseToUser(string userId, string courseId)
        {
            throw new NotImplementedException();
        }

        public ActionResponse DeleteUser(string userId)
        {
            throw new NotImplementedException();
        }

        public ActionResponse DisableUser(string userId)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public ActionResponse GetUserStatistics(string userId)
        {
            throw new NotImplementedException();
        }

        public ActionResponse Register(RegisterDto model, string role)
        {
            throw new NotImplementedException();
        }

        public ActionResponse RemoveCourseFromUser(string userId, string courseId)
        {
            throw new NotImplementedException();
        }

        public ActionResponse ResetPassword(string userId, string newPassword)
        {
            throw new NotImplementedException();
        }

        public ActionResponse UpdateUser(string userId, UpdateUserDto model)
        {
            throw new NotImplementedException();
        }
    }
}
