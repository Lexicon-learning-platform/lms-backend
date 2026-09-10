using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController(IAdminService service) : ControllerBase
    {
        // POST: api/admin/register?role=Teacher
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto model, [FromQuery] string role = "Student")
        {
            var result = service.Register(model, role);

            if (result == ActionResponse.Success)
                return Ok("User registered successfully.");
            else if (result == ActionResponse.UserAlreadyExists)
                return Conflict("User already exists.");
            else if (result == ActionResponse.InvalidRole)
                return BadRequest("Invalid role specified.");
            else
                return BadRequest("Registration failed.");
        }

        // GET: api/admin/getusers
        [HttpGet("getusers")]
        public IActionResult GetAllUsers()
        {
            var users = service.GetAllUsers();
            return Ok(users);
        }

        // DELETE: api/admin/deleteuser/{userId}
        [HttpDelete("deleteuser/{userId}")]
        public IActionResult DeleteUser(string userId)
        {
            var result = service.DeleteUser(userId);

            if (result == ActionResponse.Success)
                return Ok("User deleted successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else
                return BadRequest("Deletion failed.");
        }

        // PUT: api/admin/disableuser/{userId}
        [HttpPut("disableuser/{userId}")]
        public IActionResult DisableUser(string userId)
        {
            var result = service.DisableUser(userId);

            if (result == ActionResponse.Success)
                return Ok("User disabled successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else
                return BadRequest("Disabling user failed.");
        }

        // GET: api/admin/getuserstats/{userId}
        [HttpGet("getuserstats/{userId}")]
        public IActionResult GetUserStatistics(string userId)
        {
            var (response, statistics) = service.GetUserStatistics(userId);

            if (response == ActionResponse.Success)
                return Ok(statistics);
            else if (response == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else
                return BadRequest("Failed to retrieve user statistics.");
        }

        // PUT: api/admin/updateuser/{userId}
        [HttpPut("updateuser/{userId}")]
        public IActionResult UpdateUser(string userId, [FromBody] UpdateUserDto model)
        {
            var result = service.UpdateUser(userId, model);

            if (result == ActionResponse.Success)
                return Ok("User updated successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else if (result == ActionResponse.BadData)
                return BadRequest("Invalid data provided.");
            else if (result == ActionResponse.InvalidRole)
                return BadRequest("Invalid role specified.");
            else
                return BadRequest("Update failed.");
        }

        // PUT: api/admin/resetpassword/{userId}
        [HttpPut("resetpassword/{userId}")]
        public IActionResult ResetPassword(string userId, [FromBody] string newPassword)
        {
            var result = service.ResetPassword(userId, newPassword);

            if (result == ActionResponse.Success)
                return Ok("Password reset successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else
                return BadRequest("Password reset failed.");
        }

        // PUT: api/admin/addcoursetouser/{userId}/{courseId}
        [HttpPut("addcoursetouser/{userId}/{courseId}")]
        public IActionResult AddCourseToUser(string userId, string courseId)
        {
            var result = service.AddCourseToUser(userId, courseId);

            if (result == ActionResponse.Success)
                return Ok("Course added to user successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else if (result == ActionResponse.NotFound)
                return NotFound("Course not found.");
            else if (result == ActionResponse.BadData)
                return BadRequest("Invalid course ID provided.");
            else
                return BadRequest("Failed to add course to user.");
        }

        // PUT: api/admin/removecoursefromuser/{userId}/{courseId}
        [HttpPut("removecoursefromuser/{userId}/{courseId}")]
        public IActionResult RemoveCourseFromUser(string userId, string courseId)
        {
            var result = service.RemoveCourseFromUser(userId, courseId);

            if (result == ActionResponse.Success)
                return Ok("Course removed from user successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else if (result == ActionResponse.NotFound)
                return NotFound("Course not found.");
            else
                return BadRequest("Failed to remove course from user.");
        }


        // PUT: api/admin/enableuser/{userId}
        [HttpPut("enableuser/{userId}")]
        public IActionResult EnableUser(string userId)
        {
            var result = service.EnableUser(userId);

            if (result == ActionResponse.Success)
                return Ok("User enabled successfully.");
            else if (result == ActionResponse.UserNotFound)
                return NotFound("User not found.");
            else
                return BadRequest("Enabling user failed.");

        }
    }
}
