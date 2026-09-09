using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Mappers;
using Lms_backend.Application.Models;
using Lms_backend.Application.Validators;
using Lms_backend.Domain.Constants;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Services;

public class ModulesService(IModuleRepository repository) : IModulesService
{
    public Task<ResourceDto> AddResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AttachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
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

    public Task Remove(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task DetachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, ModuleForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, JsonPatchDocument<ModuleForChangeDto> data, CancellationToken token = default)
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
