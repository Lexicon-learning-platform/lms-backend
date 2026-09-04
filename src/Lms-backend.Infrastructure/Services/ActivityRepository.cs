using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class ActivityRepository(AppDbContext context) : RepositoryWithResourceBase<Activity, ActivityResource>(context), IActivityRepository
{
    protected override DbSet<Activity> Set => Context.Activities;
    protected override DbSet<ActivityResource> JoinSet => Context.ActivityResources;

    protected override IQueryable<ActivityResource> JoinsForEntity(Guid entityId) =>
        JoinSet.Where(j => j.ActivityId == entityId);

    protected override ActivityResource CreateJoin(Guid entityId, Guid resourceId) =>
        new() { ActivityId = entityId, ResourceId = resourceId };

    public Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesAsync(ActivitySearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetActivitiesInternalAsync(searchParams, page, pageSize, false, token);
    }

    public Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesReadOnlyAsync(ActivitySearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetActivitiesInternalAsync(searchParams, page, pageSize, true, token);
    }

    private async Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesInternalAsync(ActivitySearchParams searchParams, int page, int pageSize, bool readOnly, CancellationToken token)
    {
        var query = Set.AsSplitQuery().AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchParams.Name)) query = query.Where(a => a.Name.Contains(searchParams.Name));
        if (!string.IsNullOrWhiteSpace(searchParams.Search)) query = query.Where(a => a.Name.Contains(searchParams.Search) || a.Description.Contains(searchParams.Search));
        if (searchParams.Type.HasValue) query = query.Where(a => a.ActivityType == searchParams.Type.Value);

        var totalCount = await query.CountAsync(token);
        var pagination = new PaginationMetadata(totalCount, pageSize, page);

        var activities = await query
            .OrderBy(a => a.StartTimeOffset).ThenBy(a => a.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return (activities, pagination);
    }

    public Task<Activity?> GetActivityAsync(Guid id, CancellationToken token)
    {
        return GetActivityInternalAsync(id, false, token);
    }

    public Task<Activity?> GetActivityReadOnlyAsync(Guid id, CancellationToken token)
    {
        return GetActivityInternalAsync(id, true, token);
    }

    private async Task<Activity?> GetActivityInternalAsync(Guid id, bool readOnly, CancellationToken token)
    {
        var query = Set
            .Include(a => a.Resources).ThenInclude(ar => ar.Resource)
            .AsSplitQuery()
            .AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(a => a.Id == id, token);
    }
}
