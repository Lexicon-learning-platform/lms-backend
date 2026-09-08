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

    public async Task<ActivityDto> Create(Guid moduleId, Guid userId, ActivityForChangeDto data, CancellationToken token = default)
    {
        ActivityValidator.ValidateChangeDto(data);

        var entity = ActivityMapper.ToEntity(data, moduleId);
        await repository.AddAsync(entity, token);
        await repository.SaveChangesAsync(token);

        Console.WriteLine($"Created: {entity}");
        return ActivityMapper.ToStandardDto(entity);
    }

    public async Task<(IEnumerable<ActivityDto>, PaginationMetadata?)> GetMany(Guid moduleId, ActivitySearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        if (page == null || page < DefaultValues.page) page = DefaultValues.page;
        if (pageSize == null || pageSize <= 0) pageSize = DefaultValues.pageSize;

        var (entities, pagination) = await repository.GetActivitiesReadOnlyAsync(moduleId, searchParams, (int)page, (int)pageSize, token);
        return (ActivityMapper.ToStandardDto(entities), pagination);
    }

    public async Task<ActivityExtendedDto> GetOne(Guid moduleId, Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetActivityReadOnlyAsync(moduleId, id, token) ?? throw new NotFoundException($"Activity '{id}' not found");
        var resources = await repository.GetResourcesAsync(id, token) ?? [];

        return ActivityMapper.ToExtendedDto(entity, resources);
    }

    public async Task Remove(Guid moduleId, Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetActivityAsync(moduleId, id, token);
        if (entity == null) return;

        repository.Delete(entity);
        await repository.SaveChangesAsync(token);
    }

    public Task DetachResource(Guid moduleId, Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task Update(Guid moduleId, Guid id, ActivityForChangeDto data, CancellationToken token = default)
    {
        var entity = await repository.GetActivityReadOnlyAsync(moduleId, id, token) ?? throw new NotFoundException($"Activity '{id}' not found");
        await ApplyUpdateAsync(entity, data, token);
    }

    public async Task Update(Guid moduleId, Guid id, JsonPatchDocument<ActivityForChangeDto> data, CancellationToken token = default)
    {
        var entity = await repository.GetActivityReadOnlyAsync(moduleId, id, token) ?? throw new NotFoundException($"Activity '{id}' not found");

        var dto = ActivityMapper.ToChangeDto(entity);
        data.ApplyTo(dto);
        await ApplyUpdateAsync(entity, dto, token);
    }

    public Task UpdateResource(Guid moduleId, Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateResource(Guid moduleId, Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task ApplyUpdateAsync(Activity entity, ActivityForChangeDto dto, CancellationToken token)
    {
        ActivityValidator.ValidateChangeDto(dto);

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.StartTimeOffset = dto.StartOffset;
        entity.DurationMinutes = dto.Duration;
        entity.ActivityType = dto.Type;

        await repository.SaveChangesAsync(token);
    }
}
