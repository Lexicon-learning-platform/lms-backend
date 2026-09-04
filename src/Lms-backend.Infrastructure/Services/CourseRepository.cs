using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class CourseRepository(AppDbContext context) : RepositoryWithResourceBase<Course, CourseResource>(context), ICourseRepository
{
    protected override DbSet<Course> Set => Context.Courses;
    protected override DbSet<CourseResource> JoinSet => Context.CourseResources;

    protected override IQueryable<CourseResource> JoinsForEntity(Guid entityId) =>
        JoinSet.Where(j => j.CourseId == entityId);

    protected override CourseResource CreateJoin(Guid entityId, Guid resourceId) =>
        new() { CourseId = entityId, ResourceId = resourceId };

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
        var query = Set.AsSplitQuery().AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchParams.Name)) query = query.Where(c => c.Name.Contains(searchParams.Name));
        if (!string.IsNullOrWhiteSpace(searchParams.Search)) query = query.Where(c => c.Name.Contains(searchParams.Search) || c.Description.Contains(searchParams.Search));

        var totalCount = await query.CountAsync(token);
        var pagination = new PaginationMetadata(totalCount, pageSize, page);

        var courses = await query
            .OrderBy(c => c.StartDate).ThenBy(c => c.Name)
            .Include(c => c.Modules).ThenInclude(cm => cm.Module)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return (courses, pagination);
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
        var query = Set
            .Include(c => c.Modules).ThenInclude(cm => cm.Module)
            .Include(c => c.Resources).ThenInclude(cr => cr.Resource)
            .AsSplitQuery()
            .AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(c => c.Id == id, token);
    }
}
