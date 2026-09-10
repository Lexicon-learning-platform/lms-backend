using System.Text.Json;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/modules")]
public class ModulesController : ControllerBase
{

    private readonly Guid testingUserId;
    private readonly IModulesService _service;

    public ModulesController(IModulesService service)
    {
        _service = service;
        testingUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier")?.Value!);
    }

    // Base modules endpoints
    [HttpGet]
    public async Task<IActionResult> GetModules(string? name, string? search, Guid? course, int? page, int? pageSize, CancellationToken token = default)
    {
        var (result, pagination) = await _service.GetMany(new ModuleSearchParams(name, search, course), page, pageSize, token);
        if (pagination != null) Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagination));
        return Ok(result);
    }

    [HttpGet("{id}", Name = "GetModule")]
    public async Task<IActionResult> GetModule(Guid id, CancellationToken token = default)
    {
        var result = await _service.GetOne(id, token);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateModule(ModuleForChangeDto data, CancellationToken token = default)
    {
        var result = await _service.Create(data, token);
        return CreatedAtRoute("GetModule", new { result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModule(Guid id, ModuleForChangeDto data, CancellationToken token = default)
    {
        await _service.Update(id, data, token);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchModule(Guid id, JsonPatchDocument<ModuleForChangeDto> data, CancellationToken token = default)
    {
        await _service.Update(id, data, token);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveModule(Guid id, CancellationToken token = default)
    {
        await _service.Remove(id, token);
        return NoContent();
    }

    // Resource endpoints for modules
    [HttpGet("{id}/resources")]
    public async Task<IActionResult> GetModuleResources(Guid id, CancellationToken token = default)
    {
        var result = await _service.GetResources(id, token);
        return Ok(result);
    }

    [HttpGet("{id}/resources/{resourceId}", Name = "GetModuleResource")]
    public async Task<IActionResult> GetModuleResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var result = await _service.GetResource(id, resourceId, token);
        return Ok(result);
    }

    [HttpPost("{id}/resources")]
    public async Task<IActionResult> CreateModuleResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        var result = await _service.AddResource(id, testingUserId, data, token);
        return CreatedAtRoute("GetModuleResource", new { id, result.Id }, result);
    }

    [HttpPost("{id}/resources/{resourceId}")]
    public async Task<IActionResult> AttachModuleResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var attached = await _service.AttachResource(id, resourceId, token);
        return attached ? CreatedAtRoute("GetModuleResource", new { id, resourceId }, null) : NoContent();
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateModuleResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        await _service.UpdateResource(id, resourceId, data, token);
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchModuleResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        await _service.UpdateResource(id, resourceId, data, token);
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveModuleResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        await _service.DetachResource(id, resourceId, token);
        return NoContent();
    }
}
