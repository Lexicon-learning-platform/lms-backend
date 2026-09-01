using Lms_backend.Domain.Entities;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IRepositoryBase<TEntity, TJoin>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken token);
    Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token);
    Task<bool> SaveChangesAsync(TEntity entity, CancellationToken token);
    Task AddAsync(TEntity entity, CancellationToken token);
    void Delete(TEntity entity);
    Task<IList<Resource>> GetResourcesAsync(Guid id, CancellationToken token);
    Task<bool> AttachResourceAsync(Guid id, Guid resourseId, CancellationToken token);
    void DetachResource(Guid id, Guid resourceId);
}
