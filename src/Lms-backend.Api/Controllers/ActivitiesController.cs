using System.Text.Json;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/modules/{moduleId}/activities")]
public class ActivitiesController(IActivitiesService service) : ControllerBase
{
    // TODO: replace usage of this var with User.GetUserId() once auth is implemented
    private readonly Guid testingUserId = Guid.Parse("44444444-0000-0000-0000-000000000002");
    // Base activity endpoints
    [HttpGet]
    public async Task<IActionResult> GetActivities([FromRoute] Guid moduleId, [FromQuery] string? name, [FromQuery] string? search, [FromQuery] ActivityType? type, [FromQuery] int? page, [FromQuery] int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await service.GetMany(moduleId, new ActivitySearchParams(name, search, type), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetActivity")]
    public async Task<IActionResult> GetActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, CancellationToken token = default)
    {
        var result = await service.GetOne(moduleId, id, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity([FromRoute] Guid moduleId, [FromBody] ActivityForChangeDto data, CancellationToken token = default)
    {
        var result = await service.Create(moduleId, testingUserId, data, token);
        return CreatedAtRoute("GetActivity", new { moduleId, result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromBody] ActivityForChangeDto data, CancellationToken token = default)
    {
        await service.Update(moduleId, id, data, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromBody] JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        await service.Update(moduleId, id, data, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveActivity([FromRoute] Guid moduleId, [FromRoute] Guid id, CancellationToken token = default)
    {
        await service.Remove(moduleId, id, token);
        return NoContent();
    }

    // Resource endpoints for activities
    [HttpGet("{id}/resources")]
    public async Task<IActionResult> GetActivityResources([FromRoute] Guid moduleId, [FromRoute] Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}/resources/{resourceId}", Name = "GetActivityResource")]
    public async Task<IActionResult> GetActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost("{id}/resources")]
    public async Task<IActionResult> CreateActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromBody] ResourceForChangeDto data, CancellationToken token = default)
    {
        var result = await service.AddResource(moduleId, id, data, token);
        return CreatedAtRoute("GetActivityResource", new { id, result.Id }, result);
    }

    [HttpPost("{id}/resources/{resourceId}")]
    public async Task<IActionResult> AttachActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, CancellationToken token = default)
    {
        var attached = await service.AttachResource(moduleId, id, resourceId, token);
        return attached ? CreatedAtRoute("GetActivityResource", new { id, resourceId }, null) : NoContent();
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, [FromBody] ResourceForChangeDto data, CancellationToken token = default)
    {
        await service.UpdateResource(moduleId, id, resourceId, data, token);
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, [FromBody] JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await service.UpdateResource(moduleId, id, resourceId, data, token);
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveActivityResource([FromRoute] Guid moduleId, [FromRoute] Guid id, [FromRoute] Guid resourceId, CancellationToken token = default)
    {
        await service.DetachResource(moduleId, id, resourceId, token);
        return NoContent();
    }
}
