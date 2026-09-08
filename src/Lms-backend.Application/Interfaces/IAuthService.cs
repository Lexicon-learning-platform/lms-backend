using Lms_backend.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace Lms_backend.Application.Interfaces
{
    public interface IAuthService
    {
        List<JwtSecurityToken> Login(LoginModel model);

        JwtSecurityToken GetNewToken(string refreshToken);
        bool Logout(string refreshToken);
        bool RegisterStudent(RegisterModel model);
    }
}
