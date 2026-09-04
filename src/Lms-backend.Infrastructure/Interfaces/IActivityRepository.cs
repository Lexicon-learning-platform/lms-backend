using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IActivityRepository : IRepositoryWithResourceBase<Activity, ActivityResource>
{
    Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesAsync(ActivitySearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<(IEnumerable<Activity>, PaginationMetadata?)> GetActivitiesReadOnlyAsync(ActivitySearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<Activity?> GetActivityAsync(Guid id, CancellationToken token);
    Task<Activity?> GetActivityReadOnlyAsync(Guid id, CancellationToken token);
}
