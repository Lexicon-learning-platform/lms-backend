using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;

namespace Lms_backend.Application.Interfaces
{
    public interface IAuthService
    {
        Task<(List<JwtSecurityToken>? tokens, ActionResponse response)> Login(LoginDto model);

        JwtSecurityToken? GetNewToken(string refreshToken);
        Task<ActionResponse> Logout(string refreshToken);
        Task<ActionResponse> RegisterStudent(RegisterDto model);
    }
}
