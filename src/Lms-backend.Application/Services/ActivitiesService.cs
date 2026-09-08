using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Services;

public class ActivitiesService(IActivityRepository repository) : IActivitiesService
{
    public Task<ResourceDto> AddResource(Guid moduleId, Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AttachResource(Guid moduleId, Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ActivityDto> Create(Guid moduleId, ActivityForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<ActivityDto>, PaginationMetadata?)> GetMany(Guid moduleId, ActivitySearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ActivityExtendedDto> GetOne(Guid moduleId, Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid moduleId, Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DetachResource(Guid moduleId, Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid moduleId, Guid id, ActivityForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid moduleId, Guid id, JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateResource(Guid moduleId, Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateResource(Guid moduleId, Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
