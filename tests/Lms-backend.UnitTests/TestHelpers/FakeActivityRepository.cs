using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.UnitTests.TestHelpers;

// Hand-rolled stand-in for IActivityRepository, mirroring FakeResourceRepository so
// ActivitiesService can be unit tested without a mocking library or a real EF context.
// GetActivityAsync reads from TrackedActivities (the "write path"); GetActivityReadOnlyAsync/
// GetActivitiesReadOnlyAsync read from ReadOnlyActivities to mirror the real repository's
// separate untracked projection for reads. Both lookups also filter by moduleId, matching
// ActivityRepository's behavior of scoping an activity to its parent module.
public class FakeActivityRepository : IActivityRepository
{
    public List<Activity> TrackedActivities { get; } = [];
    public List<Activity> ReadOnlyActivities { get; } = [];

    public List<Activity> AddedEntities { get; } = [];
    public List<Activity> DeletedEntities { get; } = [];
    public int SaveChangesCallCount { get; private set; }
    public bool SaveChangesResult { get; set; } = true;

    public Dictionary<Guid, IList<Resource>> ResourcesByActivityId { get; } = [];
    public bool AttachResourceResult { get; set; } = true;
    public List<(Guid ActivityId, Guid ResourceId)> AttachResourceCalls { get; } = [];
    public List<(Guid ActivityId, Guid ResourceId)> DetachResourceCalls { get; } = [];

    public (IEnumerable<Activity> Entities, PaginationMetadata? Pagination) GetActivitiesResult { get; set; } = ([], null);
    public (IEnumerable<Activity> Entities, PaginationMetadata? Pagination) GetActivitiesReadOnlyResult { get; set; } = ([], null);
    public (Guid ModuleId, ActivitySearchParams SearchParams, int Page, int PageSize)? LastGetActivitiesReadOnlyCall { get; private set; }

    public bool HasOverlappingActivityResult { get; set; }
    public (Guid ModuleId, ActivityType Type, int StartOffset, int Duration, Guid? ExcludeId)? LastHasOverlappingActivityCall { get; private set; }

    public Task<bool> ExistsAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedActivities.Any(a => a.Id == id));

    public Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token) =>
        Task.FromResult<IList<Guid>>(ids.Where(id => TrackedActivities.All(a => a.Id != id)).ToList());

    public Task<bool> SaveChangesAsync(CancellationToken token)
    {
        SaveChangesCallCount++;
        return Task.FromResult(SaveChangesResult);
    }

    public Task AddAsync(Activity entity, CancellationToken token)
    {
        AddedEntities.Add(entity);
        TrackedActivities.Add(entity);
        return Task.CompletedTask;
    }

    public void Delete(Activity entity)
    {
        DeletedEntities.Add(entity);
        TrackedActivities.Remove(entity);
    }

    public Task<IList<Resource>> GetResourcesAsync(Guid id, CancellationToken token) =>
        Task.FromResult(ResourcesByActivityId.TryGetValue(id, out var resources) ? resources : (IList<Resource>)[]);

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

    public Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesAsync(Guid moduleId, ActivitySearchParams searchParams, int page, int pageSize, CancellationToken token) =>
        Task.FromResult(GetActivitiesResult);

    public Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesReadOnlyAsync(Guid moduleId, ActivitySearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        LastGetActivitiesReadOnlyCall = (moduleId, searchParams, page, pageSize);
        return Task.FromResult(GetActivitiesReadOnlyResult);
    }

    public Task<Activity?> GetActivityAsync(Guid moduleId, Guid id, CancellationToken token) =>
        Task.FromResult(TrackedActivities.FirstOrDefault(a => a.Id == id && a.ModuleId == moduleId));

    public Task<Activity?> GetActivityReadOnlyAsync(Guid moduleId, Guid id, CancellationToken token) =>
        Task.FromResult(
            ReadOnlyActivities.FirstOrDefault(a => a.Id == id && a.ModuleId == moduleId) ??
            TrackedActivities.FirstOrDefault(a => a.Id == id && a.ModuleId == moduleId));

    public Task<bool> HasOverlappingActivityAsync(Guid moduleId, ActivityType type, int startOffset, int durationMinutes, Guid? excludeId, CancellationToken token)
    {
        LastHasOverlappingActivityCall = (moduleId, type, startOffset, durationMinutes, excludeId);
        return Task.FromResult(HasOverlappingActivityResult);
    }
}
