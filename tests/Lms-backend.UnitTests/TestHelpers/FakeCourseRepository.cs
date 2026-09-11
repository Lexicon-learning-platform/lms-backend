using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.UnitTests.TestHelpers;

// Hand-rolled stand-in for ICourseRepository, mirroring FakeResourceRepository so
// CoursesService can be unit tested without a mocking library or a real EF context.
// GetCourseAsync reads from TrackedCourses (the "write path"); GetCourseReadOnlyAsync/
// GetCoursesReadOnlyAsync read from ReadOnlyCourses, mirroring the real repository's
// separate untracked projection for reads.
public class FakeCourseRepository : ICourseRepository
{
    public List<Course> TrackedCourses { get; } = [];
    public List<Course> ReadOnlyCourses { get; } = [];

    public List<Course> AddedEntities { get; } = [];
    public List<Course> DeletedEntities { get; } = [];
    public int SaveChangesCallCount { get; private set; }
    public bool SaveChangesResult { get; set; } = true;

    public Dictionary<Guid, IList<Resource>> ResourcesByCourseId { get; } = [];
    public bool AttachResourceResult { get; set; } = true;
    public List<(Guid CourseId, Guid ResourceId)> AttachResourceCalls { get; } = [];
    public List<(Guid CourseId, Guid ResourceId)> DetachResourceCalls { get; } = [];

    public (IEnumerable<Course> Entities, PaginationMetadata? Pagination) GetCoursesResult { get; set; } = ([], null);
    public (IEnumerable<Course> Entities, PaginationMetadata? Pagination) GetCoursesReadOnlyResult { get; set; } = ([], null);
    public (SearchParams SearchParams, int Page, int PageSize)? LastGetCoursesReadOnlyCall { get; private set; }

    public Course? CourseByUserId { get; set; }

    public Task<bool> ExistsAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedCourses.Any(c => c.Id == id));

    public Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token) =>
        Task.FromResult<IList<Guid>>(ids.Where(id => TrackedCourses.All(c => c.Id != id)).ToList());

    public Task<bool> SaveChangesAsync(CancellationToken token)
    {
        SaveChangesCallCount++;
        return Task.FromResult(SaveChangesResult);
    }

    public Task AddAsync(Course entity, CancellationToken token)
    {
        AddedEntities.Add(entity);
        TrackedCourses.Add(entity);
        return Task.CompletedTask;
    }

    public void Delete(Course entity)
    {
        DeletedEntities.Add(entity);
        TrackedCourses.Remove(entity);
        ReadOnlyCourses.Remove(entity);
    }

    public Task<IList<Resource>> GetResourcesAsync(Guid id, CancellationToken token) =>
        Task.FromResult(ResourcesByCourseId.TryGetValue(id, out var resources) ? resources : (IList<Resource>)[]);

    public Task<bool> AttachResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        AttachResourceCalls.Add((id, resourceId));
        return Task.FromResult(AttachResourceResult);
    }

    public Task DetachResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        DetachResourceCalls.Add((id, resourceId));
        return Task.CompletedTask;
    }

    public Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token) =>
        Task.FromResult(GetCoursesResult);

    public Task<(IEnumerable<Course>, PaginationMetadata?)> GetCoursesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        LastGetCoursesReadOnlyCall = (searchParams, page, pageSize);
        return Task.FromResult(GetCoursesReadOnlyResult);
    }

    public Task<Course?> GetCourseAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedCourses.FirstOrDefault(c => c.Id == id));

    public Task<Course?> GetCourseReadOnlyAsync(Guid id, CancellationToken token) =>
        Task.FromResult(ReadOnlyCourses.FirstOrDefault(c => c.Id == id) ?? TrackedCourses.FirstOrDefault(c => c.Id == id));

    public Task<Course?> GetCourseByUserIdReadOnlyAsync(Guid userId, CancellationToken token) =>
        Task.FromResult(CourseByUserId);
}
