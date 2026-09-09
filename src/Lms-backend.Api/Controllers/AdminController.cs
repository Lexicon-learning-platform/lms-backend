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
        [HttpPost("registerteacher")]
        public IActionResult RegisterTeacher([FromBody] RegisterDto model)
        {
            var result = service.Register(model, "Teacher");

            if (result == ActionResponse.Success)
                return Ok("Teacher registered successfully.");

            else
                return BadRequest("Registrering misslyckades.");
        }
    }
}
