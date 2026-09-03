using System.Text.Json;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/resources")]
public class ResourceController(IResourcesService service) : ControllerBase
{
    // Base resource endpoints
    [HttpGet]
    public async Task<IActionResult> GetResources(string? name, string? search, int? page, int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await service.GetMany(new SearchParams(name, search), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetResource")]
    public async Task<IActionResult> GetResource(Guid id, CancellationToken token = default)
    {
        var result = await service.GetOne(id, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResource(ResourceForChangeDto data, CancellationToken token = default)
    {
        var result = await service.Create(data, token);
        return CreatedAtRoute("GetResource", new { result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        await service.Update(id, data, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchResource(Guid id, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await service.Update(id, data, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveResource(Guid id, CancellationToken token = default)
    {
        await service.Remove(id, token);
        return NoContent();
    }
}
