using System.Text.Json;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivitiesController(IActivitiesService service) : ControllerBase
{
    // Base activity endpoints
    [HttpGet]
    public async Task<IActionResult> GetActivities(string? name, string? search, int? page, int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await service.GetMany(new SearchParams(name, search), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetActivity")]
    public async Task<IActionResult> GetActivity(Guid id, CancellationToken token = default)
    {
        var result = await service.GetOne(id, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(ActivityForChangeDto data, CancellationToken token = default)
    {
        var result = await service.Create(data, token);
        return CreatedAtRoute("GetActivity", new { result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(Guid id, ActivityForChangeDto data, CancellationToken token = default)
    {
        await service.Update(id, data, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchActivity(Guid id, JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        await service.Update(id, data, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveActivity(Guid id, CancellationToken token = default)
    {
        await service.Remove(id, token);
        return NoContent();
    }

    // Resource endpoints for activities
    [HttpGet("{id}/resources")]
    public async Task<IActionResult> GetActivityResources(Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}/resources/{resourceId}", Name = "GetActivityResource")]
    public async Task<IActionResult> GetActivityResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost("{id}/resources")]
    public async Task<IActionResult> CreateActivityResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        var result = service.AddResource(id, data, token);
        return CreatedAtRoute("GetActivityResource", new { id, result.Id }, result);
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateActivityResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        await service.UpdateResource(id, resourceId, data, token);
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchActivityResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await service.UpdateResource(id, resourceId, data, token);
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveActivityResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        await service.RemoveResource(id, resourceId, token);
        return NoContent();
    }
}
