using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class ActivityRepository(AppDbContext context) : RepositoryBase<Activity, ActivityResource>(context), IActivityRepository
{
    protected override DbSet<Activity> Set => Context.Activities;
    protected override DbSet<ActivityResource> ResourceSet => Context.ActivityResources;

    public Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetActivitiesInternalAsync(searchParams, page, pageSize, false, token);
    }

    public Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetActivitiesInternalAsync(searchParams, page, pageSize, true, token);
    }

    private async Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesInternalAsync(SearchParams searchParams, int page, int pageSize, bool readOnly, CancellationToken token)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}
