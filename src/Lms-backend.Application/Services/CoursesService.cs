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

public class CoursesService(ICourseRepository repository, IResourceRepository resourceRepository) : ICoursesService
{
    public async Task<CourseWithActivitiesDto?> GetByUserId(Guid userId, CancellationToken token = default)
    {
        var entity = await repository.GetCourseByUserIdReadOnlyAsync(userId, token);
        return entity is null ? null : CourseMapper.ToWithActivitiesDto(entity);
    }

    public async Task<ResourceDto> AddResource(Guid id, Guid userId, ResourceForChangeDto data, CancellationToken token = default)
    {
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");
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
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");

        var attached = await repository.AttachResourceAsync(entity.Id, resourceId, token);
        if (attached) await repository.SaveChangesAsync(token);

        return attached;
    }

    public async Task<CourseDto> Create(CourseForChangeDto data, CancellationToken token = default)
    {
        CourseValidator.ValidateChangeDto(data, isNew: true);

        var entity = CourseMapper.ToEntity(data);
        await repository.AddAsync(entity, token);
        await repository.SaveChangesAsync(token);

        return CourseMapper.ToStandardDto(entity);
    }

    public async Task<(IEnumerable<CourseDto>, PaginationMetadata?)> GetMany(SearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        if (page == null || page < DefaultValues.page) page = DefaultValues.page;
        if (pageSize == null || pageSize <= 0) pageSize = DefaultValues.pageSize;

        var (entities, pagination) = await repository.GetCoursesReadOnlyAsync(searchParams, (int)page, (int)pageSize, token);
        return (CourseMapper.ToStandardDto(entities), pagination);
    }

    public async Task<CourseExtendedDto> GetOne(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetCourseReadOnlyAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");
        return CourseMapper.ToExtendedDto(entity);
    }

    public Task Remove(Guid id, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public async Task DetachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");

        await repository.DetachResourceAsync(entity.Id, resourceId, token);
        await repository.SaveChangesAsync(token);
    }

    public Task Update(Guid id, CourseForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, JsonPatchDocument<CourseForChangeDto> data, CancellationToken token = default)
    {
        throw new NotImplementedException();
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
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");

        var resources = await repository.GetResourcesAsync(entity.Id, token);
        if (!resources.Any(r => r.Id == resourceId)) throw new NotFoundException($"Resource '{resourceId}' not found on course '{id}'");

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
