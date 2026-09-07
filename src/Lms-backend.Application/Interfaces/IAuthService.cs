using Lms_backend.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Lms_backend.Application.Interfaces
{
    public interface IAuthService
    {
        List<JwtSecurityToken> Login(LoginModel model);

        JwtSecurityToken GetNewToken(string refreshToken);
        Task<IActionResult> Logout();

    }
}
