using Lms_backend.Application.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace Lms_backend.Api.Controllers;

[ApiController]
[Route("api/modules")]
public class ModulesControllerController : ControllerBase
{
    // Base modules endpoints
    [HttpGet]
    public async Task<IActionResult> GetModules(CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}", Name = "GetModule")]
    public async Task<IActionResult> GetModule(Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateModule(ModuleForChangeDto data, CancellationToken token = default)
    {
        return CreatedAtRoute("GetModule", new {});
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateModule(Guid id, ModuleForChangeDto data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchModule(Guid id, JsonPatchDocument<ModuleForChangeDto> data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveModule(Guid id, CancellationToken token = default)
    {
        return NoContent();
    }

    // Resource endpoints for modules
    [HttpGet("{id}/resources")]
    public async Task<IActionResult> GetModuleResources(Guid id, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpGet("{id}/resources/{resourceId}", Name = "GetModuleResource")]
    public async Task<IActionResult> GetModuleResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        return Ok();
    }

    [HttpPost("{id}/resources")]
    public async Task<IActionResult> CreateModuleResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        return CreatedAtRoute("GetModuleResource", new {});
    }

    [HttpPut("{id}/resources/{resourceId}")]
    public async Task<IActionResult> UpdateModuleResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpPatch("{id}/resources/{resourceId}")]
    public async Task<IActionResult> PatchModuleResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        return NoContent();
    }

    [HttpDelete("{id}/resources/{resourceId}")]
    public async Task<IActionResult> RemoveModuleResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        return NoContent();
    }
}
