using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Mappers;
using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
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

    public async Task<(IEnumerable<ResourceDto>, PaginationMetadata?)> GetMany(ResourceSearchParams searchParams, int? page, int? pageSize, CancellationToken token = default)
    {
        if (page == null || page < DefaultValues.page) page = DefaultValues.page;
        if (pageSize == null || pageSize <= DefaultValues.pageSize) pageSize = DefaultValues.pageSize;

        var (entities, pagination) = await repository.GetResourcesReadOnlyAsync(searchParams, (int)page, (int)pageSize, token);
        throw new NotImplementedException();
    }

    public async Task<ResourceDto> GetOne(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetResourceReadOnlyAsync(id, token) ?? throw new NotFoundException($"Resource '{id}' not found");
        return ResourceMapper.ToStandardDto(entity);
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
