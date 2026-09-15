using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace Lms_backend.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService service, UserManager<ApplicationUser> userManager) : ControllerBase
    {

        private (ActionResponse, List<JwtSecurityToken>?, CookieOptions?) LoginInternal(LoginDto model)
        {
            var (tokens, response) = service.Login(model).Result;


            if (response != ActionResponse.Success)
                return (ActionResponse.Failure, null, null);

            if (tokens == null || tokens.Count < 2)
                return (ActionResponse.BadData, null, null);

            //Make cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = tokens[1].ValidTo
            };
            return (ActionResponse.Success, tokens, cookieOptions);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto model)
        {

            var (response, tokens, cookieOptions) = LoginInternal(model);

            if (response != ActionResponse.Success)
                return Unauthorized("Ogiltiga användaruppgifter.");

            if (tokens == null || tokens.Count < 2 || cookieOptions == null)
                return Unauthorized("Token generering misslyckades.");

            var accessToken = tokens[0];
            var refreshToken = tokens[1];

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
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await service.Logout(refreshToken);
                Response.Cookies.Delete("refreshToken");
            }

            return NoContent();
        }

        [HttpPost("register")]
        public IActionResult RegisterStudent([FromBody] RegisterDto model)
        {
            var result = service.RegisterStudent(model).Result;

            if (result == ActionResponse.Success)
            {
                var (response, tokens, cookieOptions) = LoginInternal(new LoginDto { Username = model.Username, Password = model.Password });

                if (response != ActionResponse.Success)
                    return Unauthorized("Ogiltiga användaruppgifter.");

                if (tokens == null || tokens.Count < 2 || cookieOptions == null)
                    return Unauthorized("Token generering misslyckades.");

                var accessToken = tokens[0];
                var refreshToken = tokens[1];

                Response.Cookies.Append("refreshToken", new JwtSecurityTokenHandler().WriteToken(refreshToken), cookieOptions);
                return Ok(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken)
                });
            }

            else
                return BadRequest("Registrering misslyckades.");
        }

        [HttpGet("getuser")]
        public IActionResult GetUser()
        {
            ApplicationUser? user = userManager.GetUserAsync(User).Result;

            return user==null ? NotFound() : Ok(user);
        }
        
        [Authorize]
        [HttpDelete("deregister")]
        public async Task<IActionResult> Deregister()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }
        
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var result = await userManager.ChangePasswordAsync(
                user,
                model.CurrentPassword,
                model.NewPassword
            );

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    errors = result.Errors.Select(e => e.Description)
                });
            }

            return NoContent();
        }

    }
}
