using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Lms_backend.Application.Interfaces
{
    public interface IAuthService
    {
        (List<JwtSecurityToken>? tokens, ActionResponse response) Login(LoginModel model);

        JwtSecurityToken GetNewToken(string refreshToken);
        ActionResponse Logout(string refreshToken);
        ActionResponse RegisterStudent(RegisterModel model);
    }
}
