using Lms_backend.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Services
{
    public class AuthRepository(AppDbContext context) : IAuthRepository
    {
    }
}
