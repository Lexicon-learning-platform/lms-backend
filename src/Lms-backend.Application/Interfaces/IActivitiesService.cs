using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Interfaces;

public interface IActivitiesService
{
    Task<(IEnumerable<ActivityDto>, PaginationMetadata?)> GetMany(Guid moduleId, ActivitySearchParams searchParams, int? page = DefaultValues.page, int? pageSize = DefaultValues.pageSize, CancellationToken token = default);
    Task<ActivityExtendedDto> GetOne(Guid moduleId, Guid id, CancellationToken token = default);
    Task<ActivityDto> Create(Guid moduleId, Guid userId, ActivityForChangeDto data, CancellationToken token = default);
    Task Update(Guid moduleId, Guid id, ActivityForChangeDto data, CancellationToken token = default);
    Task Update(Guid moduleId, Guid id, JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default);
    Task Remove(Guid moduleId, Guid id, CancellationToken token = default);
    Task<IEnumerable<ResourceDto>> GetResources(Guid moduleId, Guid id, CancellationToken token = default);
    Task<ResourceDto> GetResource(Guid moduleId, Guid id, Guid resourceId, CancellationToken token = default);
    Task<ResourceDto> AddResource(Guid moduleId, Guid id, Guid userId, ResourceForChangeDto data, CancellationToken token = default);
    Task<bool> AttachResource(Guid moduleId, Guid id, Guid resourceId, CancellationToken token = default);
    Task UpdateResource(Guid moduleId, Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default);
    Task UpdateResource(Guid moduleId, Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default);
    Task DetachResource(Guid moduleId, Guid id, Guid resourceId, CancellationToken token = default);
}
