using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Mappers;
using Lms_backend.Application.Models;
using Lms_backend.Application.Validators;
using Lms_backend.Domain.Constants;
using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Services;

public class ResourcesService(IResourceRepository repository) : IResourcesService
{
    public async Task<ResourceDto> Create(ResourceForChangeDto data, Guid userId, bool canModerate, CancellationToken token = default)
    {
        ResourceValidator.ValidateChangeDto(data);

        var entity = ResourceMapper.ToEntity(data, userId);

        await repository.AddAsync(entity, token);
        await repository.SaveChangesAsync(token);

        // refetch to properly fill in user info
        var createdEntity = await repository.GetResourceReadOnlyAsync(entity.Id, token);
        return ResourceMapper.ToStandardDto(createdEntity!);
    }

    public async Task<(IEnumerable<ResourceDto>, PaginationMetadata?)> GetMany(ResourceSearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        if (page == null || page < DefaultValues.page) page = DefaultValues.page;
        if (pageSize == null || pageSize <= DefaultValues.pageSize) pageSize = DefaultValues.pageSize;

        var (entities, pagination) = await repository.GetResourcesReadOnlyAsync(searchParams, (int)page, (int)pageSize, token);
        return (ResourceMapper.ToStandardDto(entities), pagination);
    }

    public async Task<ResourceDto> GetOne(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetResourceReadOnlyAsync(id, token) ?? throw new NotFoundException($"Resource '{id}' not found");
        return ResourceMapper.ToStandardDto(entity);
    }

    public async Task Remove(Guid id, Guid userId, bool canModerate, CancellationToken token = default)
    {
        var entity = await repository.GetResourceAsync(id, token);
        if (entity == null) return;
        EnsureOwnerOrModerator(entity, userId, canModerate);

        repository.Delete(entity);
        await repository.SaveChangesAsync(token);
    }

    public async Task Update(Guid id, ResourceForChangeDto data, Guid userId, bool canModerate, CancellationToken token = default)
    {
        var entity = await repository.GetResourceAsync(id, token) ?? throw new NotFoundException($"Resource '{id}' not found");
        EnsureOwnerOrModerator(entity, userId, canModerate);
        await ApplyUpdateAsync(entity, data, token);
    }

    public async Task Update(Guid id, JsonPatchDocument<ResourceForChangeDto> data, Guid userId, bool canModerate, CancellationToken token = default)
    {
        var entity = await repository.GetResourceAsync(id, token) ?? throw new NotFoundException($"Resource '{id}' not found");
        EnsureOwnerOrModerator(entity, userId, canModerate);

        var dto = ResourceMapper.ToChangeDto(entity);
        data.ApplyTo(dto);

        await ApplyUpdateAsync(entity, dto, token);
    }

    private async Task ApplyUpdateAsync(Resource entity, ResourceForChangeDto update, CancellationToken token)
    {
        ResourceValidator.ValidateChangeDto(update);

        entity.Name = update.Name;
        entity.Description = update.Description;
        entity.ResourceType = update.Type;
        entity.Data = update.Data;

        await repository.SaveChangesAsync(token);
    }

    private static void EnsureOwnerOrModerator(Resource entity, Guid currentUserId, bool canModerate)
    {
        if (canModerate) return;

        if (entity.OwnerId != currentUserId) throw new ForbiddenException("You can only modify your own resources");
    }
}
