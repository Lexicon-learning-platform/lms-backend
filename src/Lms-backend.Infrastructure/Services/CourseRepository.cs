using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class CourseRepository(AppDbContext context) : RepositoryBase<Course, CourseResource>(context), ICourseRepository
{
    protected override DbSet<Course> Set => Context.Courses;
    protected override DbSet<CourseResource> JoinSet => Context.CourseResources;

    public Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetCoursesInternalAsync(searchParams, page, pageSize, false, token);
    }

    public Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetCoursesInternalAsync(searchParams, page, pageSize, true, token);
    }

    private async Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesInternalAsync(SearchParams searchParams, int page, int pageSize, bool readOnly, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<Course?> GetCourseAsync(Guid id, CancellationToken token)
    {
        return GetCourseInternalAsync(id, false, token);
    }

    public Task<Course?> GetCourseReadOnlyAsync(Guid id, CancellationToken token)
    {
        return GetCourseInternalAsync(id, true, token);
    }

    private async Task<Course?> GetCourseInternalAsync(Guid id, bool readOnly, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
