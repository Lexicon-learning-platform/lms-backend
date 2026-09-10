using System.Text.Json;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/modules/{moduleId}/activities")]
[Authorize]
public class ActivitiesController : ControllerBase
{

    private readonly Guid testingUserId;
    private readonly IActivitiesService _service;

    public ActivitiesController(IActivitiesService service)
    {
        _service = service;
        testingUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value!);
    }

    // Base course endpoints
    [HttpGet]
    public async Task<IActionResult> GetActivities([FromRoute] Guid moduleId, [FromQuery] string? name, [FromQuery] string? search, [FromQuery] ActivityType? type, [FromQuery] int? page, [FromQuery] int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await _service.GetMany(moduleId, new ActivitySearchParams(name, search, type), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetActivity")]
    public async Task<IActionResult> GetActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, CancellationToken token = default)
    {
        var result = await _service.GetOne(moduleId, id, token);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> CreateActivity([FromRoute] Guid moduleId, [FromBody] ActivityForChangeDto data, CancellationToken token = default)
    {
        var result = await _service.Create(moduleId, testingUserId, data, token);
        return CreatedAtRoute("GetActivity", new { moduleId, id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> UpdateActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromBody] ActivityForChangeDto data, CancellationToken token = default)
    {
        await _service.Update(moduleId, id, data, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> PatchActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromBody] JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        await _service.Update(moduleId, id, data, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> RemoveActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, CancellationToken token = default)
    {
        await _service.Remove(moduleId, id, token);
        return NoContent();
    }

    // Resource endpoints for activities
    [HttpGet("{id}/resources")]
    public async Task<IActionResult> GetActivityResources([FromRoute] Guid moduleId, [FromRoute] Guid id, CancellationToken token = default)
    {
        var result = await _service.GetResources(moduleId, id, token);
        return Ok(result);
    }

    [HttpGet("{id}/resources/{resourceId}", Name = "GetActivityResource")]
    public async Task<IActionResult> GetActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, CancellationToken token = default)
    {
        var result = await _service.GetResource(moduleId, id, resourceId, token);
        return Ok(result);
    }

    [HttpPost("{id}/resources")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> CreateActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromBody] ResourceForChangeDto data, CancellationToken token = default)
    {
        var result = await _service.AddResource(moduleId, id, testingUserId, data, token);
        return CreatedAtRoute("GetActivityResource", new { moduleId, id, resourceId = result.Id }, result);
    }

    [HttpPost("{id}/resources/{resourceId}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> AttachActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, CancellationToken token = default)
    {
        var attached = await _service.AttachResource(moduleId, id, resourceId, token);
        return attached ? CreatedAtRoute("GetActivityResource", new { moduleId, id, resourceId }, null) : NoContent();
    }

    [HttpPut("{id}/resources/{resourceId}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> UpdateActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, [FromBody] ResourceForChangeDto data, CancellationToken token = default)
    {
        await _service.UpdateResource(moduleId, id, resourceId, data, token);
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> PatchActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, [FromBody] JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await _service.UpdateResource(moduleId, id, resourceId, data, token);
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    [Authorize(Roles = Roles.TeacherAndAbove)]
    public async Task<IActionResult> RemoveActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, CancellationToken token = default)
    {
        await _service.DetachResource(moduleId, id, resourceId, token);
        return NoContent();
    }
}
