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

public class ModulesService(IModuleRepository repository, IResourceRepository resourceRepository) : IModulesService
{
    public async Task<IEnumerable<ResourceDto>> GetResources(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetModuleReadOnlyAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");
        return ResourceMapper.ToStandardDto(entity.Resources.Select(r => r.Resource));
    }

    public async Task<ResourceDto> GetResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var entity = await repository.GetModuleReadOnlyAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");
        var resource = entity.Resources.FirstOrDefault(r => r.ResourceId == resourceId)?.Resource ?? throw new NotFoundException($"Resource '{resourceId}' not found on module '{id}'");
        return ResourceMapper.ToStandardDto(resource);
    }

    public async Task<ResourceDto> AddResource(Guid id, Guid userId, ResourceForChangeDto data, CancellationToken token = default)
    {
        var entity = await repository.GetModuleAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");
        ResourceValidator.ValidateChangeDto(data);

        var resource = ResourceMapper.ToEntity(data, userId);
        await resourceRepository.AddAsync(resource, token);
        await repository.AttachResourceAsync(entity.Id, resource.Id, token);
        await repository.SaveChangesAsync(token);

        var createdResource = await resourceRepository.GetResourceReadOnlyAsync(resource.Id, token);
        return ResourceMapper.ToStandardDto(createdResource!);
    }

    public async Task<bool> AttachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var entity = await repository.GetModuleAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");

        var attached = await repository.AttachResourceAsync(entity.Id, resourceId, token);
        if (attached) await repository.SaveChangesAsync(token);

        return attached;
    }

    public async Task<ModuleDto> Create(ModuleForChangeDto data, CancellationToken token = default)
    {
        ModuleValidator.ValidateChangeDto(data);

        var entity = ModuleMapper.ToEntity(data);
        await repository.AddAsync(entity, token);
        await repository.SaveChangesAsync(token);

        return ModuleMapper.ToStandardDto(entity);
    }

    public async Task<(IEnumerable<ModuleDto>, PaginationMetadata?)> GetMany(ModuleSearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        if (page == null || page < DefaultValues.page) page = DefaultValues.page;
        if (pageSize == null || pageSize <= 0) pageSize = DefaultValues.pageSize;

        var (entities, pagination) = await repository.GetModulesReadOnlyAsync(searchParams, (int)page, (int)pageSize, token);
        return (ModuleMapper.ToStandardDto(entities), pagination);
    }

    public async Task<ModuleExtendedDto> GetOne(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetModuleReadOnlyAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");
        return ModuleMapper.ToExtendedDto(entity, null);
    }

    public async Task Remove(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetModuleAsync(id, token);
        if (entity == null) return;

        repository.Delete(entity);
        await repository.SaveChangesAsync(token);
    }

    public async Task DetachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var entity = await repository.GetModuleAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");

        await repository.DetachResourceAsync(entity.Id, resourceId, token);
        await repository.SaveChangesAsync(token);
    }

    public async Task Update(Guid id, ModuleForChangeDto data, CancellationToken token = default)
    {
        var entity = await repository.GetModuleAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");
        await ApplyUpdateAsync(entity, data, token);
    }

    public async Task Update(Guid id, JsonPatchDocument<ModuleForChangeDto> data, CancellationToken token = default)
    {
        var entity = await repository.GetModuleAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");

        var dto = ModuleMapper.ToChangeDto(entity);
        data.ApplyTo(dto);

        await ApplyUpdateAsync(entity, dto, token);
    }

    private async Task ApplyUpdateAsync(Module entity, ModuleForChangeDto update, CancellationToken token)
    {
        ModuleValidator.ValidateChangeDto(update);

        entity.Name = update.Name;
        entity.Description = update.Description;
        entity.Duration = update.Duration;

        await repository.SaveChangesAsync(token);
    }

    public async Task UpdateResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        var resource = await GetAttachedResourceAsync(id, resourceId, token);
        await ApplyResourceUpdateAsync(resource, data, token);
    }

    public async Task UpdateResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        var resource = await GetAttachedResourceAsync(id, resourceId, token);

        var dto = ResourceMapper.ToChangeDto(resource);
        data.ApplyTo(dto);
        await ApplyResourceUpdateAsync(resource, dto, token);
    }

    private async Task<Resource> GetAttachedResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        var entity = await repository.GetModuleAsync(id, token) ?? throw new NotFoundException($"Module '{id}' not found");

        var resources = await repository.GetResourcesAsync(entity.Id, token);
        if (!resources.Any(r => r.Id == resourceId)) throw new NotFoundException($"Resource '{resourceId}' not found on module '{id}'");

        return await resourceRepository.GetResourceAsync(resourceId, token) ?? throw new NotFoundException($"Resource '{resourceId}' not found");
    }

    private async Task ApplyResourceUpdateAsync(Resource entity, ResourceForChangeDto update, CancellationToken token)
    {
        ResourceValidator.ValidateChangeDto(update);

        entity.Name = update.Name;
        entity.Description = update.Description;
        entity.ResourceType = update.Type;
        entity.Data = update.Data;

        await resourceRepository.SaveChangesAsync(token);
    }
}
