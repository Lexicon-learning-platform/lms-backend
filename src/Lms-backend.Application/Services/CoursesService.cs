using Lms_backend.Application.Exceptions;
using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Mappers;
using Lms_backend.Application.Models;
using Lms_backend.Application.Validators;
using Lms_backend.Domain.Constants;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using System.Reflection.Metadata.Ecma335;

namespace Lms_backend.Application.Services;

public class CoursesService(ICourseRepository repository, IResourceRepository resourceRepository, IModuleRepository moduleRepository, UserManager<ApplicationUser> userManager) : ICoursesService
{
    public async Task<CourseWithActivitiesDto?> GetByUserId(Guid userId, CancellationToken token = default)
    {
        var entity = await repository.GetCourseByUserIdReadOnlyAsync(userId, token);
        return entity is null ? null : CourseMapper.ToWithActivitiesDto(entity);
    }

    public async Task<IEnumerable<ResourceDto>> GetResources(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetCourseReadOnlyAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");
        return ResourceMapper.ToStandardDto(entity.Resources.Select(r => r.Resource));
    }

    public async Task<ResourceDto> GetResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var entity = await repository.GetCourseReadOnlyAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");
        var resource = entity.Resources.FirstOrDefault(r => r.ResourceId == resourceId)?.Resource ?? throw new NotFoundException($"Resource '{resourceId}' not found on course '{id}'");
        return ResourceMapper.ToStandardDto(resource);
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

        var orderedModules = await GetOrderedModulesAsync(data.ModuleIds, token);
        ValidateModuleDurations(orderedModules, data.Duration);

        var entity = CourseMapper.ToEntity(data);
        entity.Modules = BuildModuleJoins(orderedModules);

        await repository.AddAsync(entity, token);
        await repository.SaveChangesAsync(token);

        var createdCourse = await repository.GetCourseReadOnlyAsync(entity.Id, token);
        return CourseMapper.ToStandardDto(createdCourse!);
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

    public async Task Remove(Guid id, CancellationToken token = default)
    {
        var entity = await repository.GetCourseReadOnlyAsync(id, token);
        if (entity == null) return;

        repository.Delete(entity);
        await repository.SaveChangesAsync(token);
    }

    public async Task DetachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");

        await repository.DetachResourceAsync(entity.Id, resourceId, token);
        await repository.SaveChangesAsync(token);
    }

    public async Task Update(Guid id, CourseForChangeDto data, CancellationToken token = default)
    {
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");
        await ApplyUpdateAsync(entity, data, token);
    }

    public async Task Update(Guid id, JsonPatchDocument<CourseForChangeDto> data, CancellationToken token = default)
    {
        var entity = await repository.GetCourseAsync(id, token) ?? throw new NotFoundException($"Course '{id}' not found");

        var dto = CourseMapper.ToChangeDto(entity);
        data.ApplyTo(dto);

        await ApplyUpdateAsync(entity, dto, token);
    }

    private async Task ApplyUpdateAsync(Course entity, CourseForChangeDto update, CancellationToken token)
    {
        CourseValidator.ValidateChangeDto(update, isNew: false);

        var orderedModules = await GetOrderedModulesAsync(update.ModuleIds, token);
        ValidateModuleDurations(orderedModules, update.Duration);

        entity.Name = update.Name;
        entity.Description = update.Description;
        entity.StartDate = update.StartDate;
        entity.Duration = update.Duration;

        SyncModules(entity, orderedModules);

        await repository.SaveChangesAsync(token);
    }

    // Fetches the requested modules (deduplicated, order preserved) and fails
    // fast if any id doesn't exist - callers use the result to compute each
    // module's sequential StartTimeOffset from its Duration.
    private async Task<IReadOnlyList<Module>> GetOrderedModulesAsync(Guid[] moduleIds, CancellationToken token)
    {
        var distinctIds = moduleIds.Distinct().ToArray();
        if (distinctIds.Length == 0) return [];

        var missing = await moduleRepository.GetMissingIdsAsync(distinctIds, token);
        if (missing.Count > 0) throw new NotFoundException($"Module(s) '{string.Join(", ", missing)}' not found");

        var modules = await moduleRepository.GetModulesByIdsAsync(distinctIds, token);
        var modulesById = modules.ToDictionary(m => m.Id);

        return [.. distinctIds.Select(id => modulesById[id])];
    }

    private static void ValidateModuleDurations(IReadOnlyList<Module> orderedModules, int courseDuration)
    {
        var totalDuration = orderedModules.Sum(m => m.Duration);
        if (totalDuration > courseDuration)
        {
            throw new ValidationException(
                $"Modules' total duration ({totalDuration}) exceeds the course duration ({courseDuration})");
        }
    }

    // Modules are placed back-to-back in the given order: each one starts
    // right after the previous one ends, so they can never overlap.
    private static ICollection<CourseModule> BuildModuleJoins(IReadOnlyList<Module> orderedModules)
    {
        var joins = new List<CourseModule>();
        var offset = 0;

        foreach (var module in orderedModules)
        {
            joins.Add(new CourseModule { ModuleId = module.Id, StartTimeOffset = offset });
            offset += module.Duration;
        }

        return joins;
    }

    private static void SyncModules(Course entity, IReadOnlyList<Module> orderedModules)
    {
        entity.Modules.Clear();

        var offset = 0;
        foreach (var module in orderedModules)
        {
            entity.Modules.Add(new CourseModule { CourseId = entity.Id, ModuleId = module.Id, StartTimeOffset = offset });
            offset += module.Duration;
        }
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

    public async Task<List<ApplicationUser>?> GetClassmates(Guid currentUserId, CancellationToken token)
    {
        var course = await repository.GetCourseAsync(currentUserId, token);

        if (course != null)
        {
            var students = course.Users.ToList();
            return students;
        }
        else return null;
    }
}
