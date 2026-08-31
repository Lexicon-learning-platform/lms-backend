using Lms_backend.Application.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivitiesController : ControllerBase
{
    // Base activity endpoints
    [HttpGet]
    public async Task<IActionResult> GetActivities(CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}", Name = "GetActivity")]
    public async Task<IActionResult> GetActivity(Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity(ActivityForChangeDto data, CancellationToken token = default)
    {
        return CreatedAtRoute("GetActivity", new {});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateActivity(Guid id, ActivityForChangeDto data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchActivity(Guid id, JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveActivity(Guid id, CancellationToken token = default)
    {
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
        return CreatedAtRoute("GetActivityResource", new {});
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateActivityResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchActivityResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveActivityResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        return NoContent();
    }
}
