using Lms_backend.Application.Interfaces;
using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Lms_backend.Application.Services
{
    public class AuthService(IAuthRepository repository, IConfiguration configuration) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        public List<JwtSecurityToken> Login(LoginModel model)
        {
            //Check if the user exists and the password is correct

            // För enkelhetens skull kör vi en hårdkodad kontroll (ersätt med databas)
            if (model.Username != "admin" || model.Password != "hemligt")
                return null;

            //Build access and refresh tokens

            var claims = new List<Claim>
                {
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
            //TODO: Spara refresh token i databas istället för i minnet

            string tokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            repository.StoreRefreshTokenAsync(tokenString, model.Username);

            return new List<JwtSecurityToken> { accessToken, refreshToken };
        }


        public JwtSecurityToken GetNewToken(string refreshToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(refreshToken);
            if (!repository.IsRefreshTokenValidAsync(refreshToken).Result)
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

        public bool Logout(string refreshToken)
        {
            //Delete the refresh token from the database or in-memory list
            return repository.RevokeRefreshTokenAsync(refreshToken).Result;
        }

    }
}
