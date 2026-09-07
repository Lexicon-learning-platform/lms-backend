using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Lms_backend.Application.Interfaces;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService service, IConfiguration configuration) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly string ACCESS_TOKEN_SECRET = "youraccesstokensecret";
        private readonly string REFRESH_TOKEN_SECRET = "yourrefreshtokensecret";

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            
            var result = service.Login(model);


            if (result == null)
                return Unauthorized("Ogiltiga användaruppgifter.");

            var accessToken = result[0];
            var refreshToken = result[1];

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

            var newAccessToken = service.GetNewToken(refreshToken);

            if (newAccessToken == null)
                return Unauthorized("Ogiltig refresh token.");

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken)
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            bool result;
            var refreshToken = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(refreshToken))
            {
                result = service.Logout(refreshToken);
                Response.Cookies.Delete("refreshToken");
            }
            return Ok("Utloggad.");
        }

    }
}
