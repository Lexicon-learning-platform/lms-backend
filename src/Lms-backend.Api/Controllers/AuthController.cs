using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Lms_backend.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly string ACCESS_TOKEN_SECRET = "youraccesstokensecret";
        private readonly string REFRESH_TOKEN_SECRET = "yourrefreshtokensecret";

        private List<JwtSecurityToken> refreshTokens = new List<JwtSecurityToken>();



        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            // För enkelhetens skull kör vi en hårdkodad kontroll (ersätt med databas)
            if (model.Username != "admin" || model.Password != "hemligt")
                return Unauthorized("Ogiltiga användaruppgifter.");

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, model.Role)
                };

            var accessKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:AccessSecret"]!));
            var accessCreds = new SigningCredentials(accessKey, SecurityAlgorithms.HmacSha256);

            var refreshKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:RefreshSecret"]!));
            var refreshCreds = new SigningCredentials(refreshKey, SecurityAlgorithms.HmacSha256);

            // Skapa själva tokenet
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

            refreshTokens.Add(refreshToken);

            //Make cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.ValidTo
            };
            Response.Cookies.Append("refreshToken", new JwtSecurityTokenHandler().WriteToken(refreshToken), cookieOptions);
            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken)
            });
        }

        [HttpPost("token")]
        public IActionResult Token()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Ingen refresh token hittades.");
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(refreshToken);
            if (!refreshTokens.Any(t => t.RawData == refreshToken))
                return Unauthorized("Ogiltig refresh token.");
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
            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken)
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                refreshTokens.RemoveAll(t => t.RawData == refreshToken);
                Response.Cookies.Delete("refreshToken");
            }
            return Ok("Utloggad.");
        }


        public class LoginModel
        {
            public string Username { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
    }
}
