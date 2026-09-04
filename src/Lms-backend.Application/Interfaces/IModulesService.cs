using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Interfaces;

public interface IModulesService
{
    Task<(IEnumerable<ModuleDto>, PaginationMetadata?)> GetMany(ModuleSearchParams searchParams, int? page = DefaultValues.page, int? pageSize = DefaultValues.pageSize, CancellationToken token = default);
    Task<ModuleExtendedDto> GetOne(Guid id, CancellationToken token = default);
    Task<ModuleDto> Create(ModuleForChangeDto data, CancellationToken token = default);
    Task Update(Guid id, ModuleForChangeDto data, CancellationToken token = default);
    Task Update(Guid id, JsonPatchDocument<ModuleForChangeDto> data, CancellationToken token = default);
    Task Remove(Guid id, CancellationToken token = default);
    Task<ResourceDto> AddResource(Guid id, ResourceForChangeDto data, CancellationToken token = default);
    Task<bool> AttachResource(Guid id, Guid resourceId, CancellationToken token = default);
    Task UpdateResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default);
    Task UpdateResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default);
    Task DetachResource(Guid id, Guid resourceId, CancellationToken token = default);
}
