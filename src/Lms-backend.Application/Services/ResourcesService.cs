using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Services;

public class ResourcesService(IResourceRepository repository) : IResourcesService
{
    public Task<ResourceDto> Create(ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<ResourceDto>, PaginationMetadata?)> GetMany(SearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResourceDto> GetOne(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }
}
