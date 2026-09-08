using Lms_backend.Application.Interfaces;
using Lms_backend.Application.Mappers;
using Lms_backend.Application.Models;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Services;

public class CoursesService(ICourseRepository repository) : ICoursesService
{
    
    public async Task<CourseWithActivitiesDto?> GetByUserId(Guid userId, CancellationToken token = default)
    {
        var entity = await repository.GetCourseByUserIdReadOnlyAsync(userId, token);
        return entity is null ? null : CourseMapper.ToWithActivitiesDto(entity);
    }
    
    
    public Task<ResourceDto> AddResource(Guid id, ResourceForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AttachResource(Guid id, Guid resourceId, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<CourseDto> Create(CourseForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<CourseDto>, PaginationMetadata?)> GetMany(SearchParams searchParams, int? page = 1, int? pageSize = 10, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task<CourseExtendedDto> GetOne(Guid id, CancellationToken token = default)
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

    public Task Update(Guid id, CourseForChangeDto data, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, JsonPatchDocument<CourseForChangeDto> data, CancellationToken token = default)
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
