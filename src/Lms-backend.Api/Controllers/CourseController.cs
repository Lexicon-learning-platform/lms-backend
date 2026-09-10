using System.Text.Json;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/courses")]
public class CourseController : ControllerBase
{

    private readonly Guid testingUserId;
    private readonly ICoursesService _service;

    public CourseController(ICoursesService service)
    {
        _service = service;
        testingUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value!);
    }

    // Base course endpoints
    [HttpGet("my-course")]
    public async Task<IActionResult> GetCurrentUserCourse(CancellationToken token = default)
    {
        var result = await _service.GetByUserId(testingUserId, token);
        return Ok(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetCourses(string? name, string? search, int? page, int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await _service.GetMany(new SearchParams(name, search), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetCourse")]
    public async Task<IActionResult> GetCourse(Guid id, CancellationToken token = default)
    {
        var result = await _service.GetOne(id, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse(CourseForChangeDto data, CancellationToken token = default)
    {
        var result = await _service.Create(data, token);
        return CreatedAtRoute("GetCourse", new { result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCourse(Guid id, CourseForChangeDto data, CancellationToken token = default)
    {
        await _service.Update(id, data, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchCourse(Guid id, JsonPatchDocument<CourseForChangeDto> data, CancellationToken token = default)
    {
        await _service.Update(id, data, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveCourse(Guid id, CancellationToken token = default)
    {
        await _service.Remove(id, token);
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
        var result = await _service.AddResource(id, data, token);
        return CreatedAtRoute("GetCourseResource", new { id, result.Id }, result);
    }

    [HttpPost("{id}/resources/{resourceId}")]
    public async Task<IActionResult> AttachCourseResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var attached = await _service.AttachResource(id, resourceId, token);
        return attached ? CreatedAtRoute("GetCourseResource", new { id, resourceId }, null) : NoContent();
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateCourseResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        await _service.UpdateResource(id, resourceId, data, token);
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchCourseResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await _service.UpdateResource(id, resourceId, data, token);
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveCourseResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        await _service.DetachResource(id, resourceId, token);
        return NoContent();
    }
}
