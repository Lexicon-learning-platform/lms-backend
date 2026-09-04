using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Services;

public class ActivitiesService(IActivityRepository repository) : IActivitiesService
{
    public Task<ResourceDto> AddResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AttachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ActivityDto> Create(ActivityForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<ActivityDto>, PaginationMetadata?)> GetMany(ActivitySearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ActivityExtendedDto> GetOne(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DetachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, ActivityForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
