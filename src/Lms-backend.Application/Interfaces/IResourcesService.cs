using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Interfaces;

public interface IResourcesService
{
    Task<(IEnumerable<ResourceDto>, PaginationMetadata?)> GetMany(ResourceSearchParams searchParams, int? page = DefaultValues.page, int? pageSize = DefaultValues.pageSize, CancellationToken token = default);
    Task<ResourceDto> GetOne(Guid id, CancellationToken token = default);
    Task<ResourceDto> Create(ResourceForChangeDto data, CancellationToken token = default);
    Task Update(Guid id, ResourceForChangeDto data, CancellationToken token = default);
    Task Update(Guid id, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default);
    Task Remove(Guid id, CancellationToken token = default);
}
