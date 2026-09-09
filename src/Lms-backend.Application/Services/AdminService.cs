using Lms_backend.Application.Interfaces;
using Lms_backend.Domain.Entities;
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


    }
}
