using Lms_backend.Application.Models;
using Lms_backend.Domain.Constants;
using Lms_backend.Infrastructure.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Lms_backend.Application.Interfaces;

public interface ICoursesService
{
    Task<(IEnumerable<CourseDto>, PaginationMetadata?)> GetMany(SearchParams searchParams, int? page = DefaultValues.page, int? pageSize = DefaultValues.pageSize, CancellationToken token = default);
    Task<CourseExtendedDto> GetOne(Guid id, CancellationToken token = default);
    Task<CourseDto> Create(CourseForChangeDto data, CancellationToken token = default);
    Task Update(Guid id, CourseForChangeDto data, CancellationToken token = default);
    Task Update(Guid id, JsonPatchDocument<CourseForChangeDto> data, CancellationToken token = default);
    Task Remove(Guid id, CancellationToken token = default);
    Task<ResourceDto> AddResource(Guid id, ResourceForChangeDto data, CancellationToken token = default);
    Task UpdateResource(Guid id, Guid resourceId, ResourceForChangeDto data, CancellationToken token = default);
    Task UpdateResource(Guid id, Guid resourceId, JsonPatchDocument<ResourceForChangeDto> data, CancellationToken token = default);
    Task RemoveResource(Guid id, Guid resourceId, CancellationToken token = default);
}
