using Lms_backend.Application.Interfaces;
using Lms_backend.Domain.Entities;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lms_backend.Application.Services
{
    public class AuthService(IAuthRepository repository, IConfiguration configuration, UserManager<ApplicationUser> userManager) : IAuthService
    {
        private readonly IAuthRepository _repository = repository;
        private readonly IConfiguration _configuration = configuration;
        private readonly UserManager<ApplicationUser> _userManager = userManager;


        public async Task<(List<JwtSecurityToken>? tokens, ActionResponse response)> Login(LoginDto model)
        {
            //Check if the user exists and the password is correct
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user==null) return (null, ActionResponse.UserNotFound);

            var legit = await _userManager.CheckPasswordAsync(user, model.Password);
            if(!legit) return (null, ActionResponse.PasswordMismatch);

            //Build access and refresh tokens
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, model.Role)
                };

            var accessKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:AccessSecret"]!));
            var accessCreds = new SigningCredentials(accessKey, SecurityAlgorithms.HmacSha256);

            var refreshKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshSecret"]!));
            var refreshCreds = new SigningCredentials(refreshKey, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: accessCreds
            );

            var refreshToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: refreshCreds
);

            //TODO: Get user ID from database and store it with the refresh token in the database or in-memory list

            string tokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            await _repository.StoreRefreshTokenAsync(tokenString, user.Id);

            return (new List<JwtSecurityToken> { accessToken, refreshToken }, ActionResponse.Success);
        }


        public JwtSecurityToken? GetNewToken(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(refreshToken);
            if (!_repository.IsRefreshTokenValidAsync(refreshToken).Result)
                return null;
            var claims = token.Claims.ToList();
            var accessKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:AccessSecret"]!));
            var accessCreds = new SigningCredentials(accessKey, SecurityAlgorithms.HmacSha256);
            var newAccessToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: accessCreds
            );

            return newAccessToken;

        }

        public async Task<ActionResponse> Logout(string refreshToken)
        {
            //Delete the refresh token from the database or in-memory list
            return await _repository.RevokeRefreshTokenAsync(refreshToken);
        }

        public async Task<ActionResponse> RegisterStudent(RegisterDto model)
        {
            //Check if the user already exists
            var existingUser = await _userManager.FindByNameAsync(model.Username);
            if (existingUser != null)
                return ActionResponse.UserAlreadyExists;

            //Create a new user and store it in the database

            ApplicationUser newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = model.Username,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Role = "Student"
            };

            await _userManager.CreateAsync(newUser);

            await _userManager.AddPasswordAsync(newUser, model.Password);

            return ActionResponse.Success;
        }
    }
}
