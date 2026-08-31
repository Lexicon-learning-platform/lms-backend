using Lms_backend.Application.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{
    // Base course endpoints
    [HttpGet]
    public async Task<IActionResult> GetCourses(CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}", Name = "GetCourse")]
    public async Task<IActionResult> GetCourse(Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseForChangeDto data, CancellationToken token = default)
    {
        return CreatedAtRoute("GetCourse", new {});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(Guid id, CourseForChangeDto data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCourse(Guid id, JsonPatchDocument<CourseForChangeDto> data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveCourse(Guid id, CancellationToken token = default)
    {
        return NoContent();
    }

    // Resource endpoints for courses
    [HttpGet("{id}/resources")]
    public async Task<IActionResult> GetCourseResources(Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}/resources/{resourceId}", Name = "GetCourseResource")]
    public async Task<IActionResult> GetCourseResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost("{id}/resources")]
    public async Task<IActionResult> CreateCourseResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        return CreatedAtRoute("GetCourseResource", new {});
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateCourseResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchCourseResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveCourseResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        return NoContent();
    }
}
