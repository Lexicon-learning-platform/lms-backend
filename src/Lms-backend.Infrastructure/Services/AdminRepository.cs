using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Services
{
    public class AdminRepository(UserManager<ApplicationUser> userManager) : IAdminRepository
    {
    }
}
