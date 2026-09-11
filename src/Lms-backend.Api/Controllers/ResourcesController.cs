using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/resources")]
[Authorize]
public class ResourceController : ControllerBase
{
    private readonly IResourcesService _service;

    public ResourceController(IResourcesService service)
    {
        _service = service;
    }

    private Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // Base resource endpoints
    [HttpGet]
    public async Task<IActionResult> GetResources(string? name, string? search, ResourceType type, int? page, int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await _service.GetMany(new ResourceSearchParams(name, search, type), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetResource")]
    public async Task<IActionResult> GetResource(Guid id, CancellationToken token = default)
    {
        var result = await _service.GetOne(id, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResource(ResourceForChangeDto data, CancellationToken token = default)
    {
        var result = await _service.Create(data, CurrentUserId, CanModerate, token);
        return CreatedAtRoute("GetResource", new { result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        await _service.Update(id, data, CurrentUserId, CanModerate, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchResource(Guid id, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await _service.Update(id, data, CurrentUserId, CanModerate, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveResource(Guid id, CancellationToken token = default)
    {
        await _service.Remove(id, CurrentUserId, CanModerate, token);
        return NoContent();
    }

    private bool CanModerate => User.IsInRole(Roles.Admin);
}
