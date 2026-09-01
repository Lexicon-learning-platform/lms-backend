using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.Infrastructure.Interfaces;

public interface ICourseRepository : IRepositoryBase<Course, CourseResource>
{
    Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<Course?> GetCourseAsync(Guid id, CancellationToken token);
    Task<Course?> GetCourseReadOnlyAsync(Guid id, CancellationToken token);
}
