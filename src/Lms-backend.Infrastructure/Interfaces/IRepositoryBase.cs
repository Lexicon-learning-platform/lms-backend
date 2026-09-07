using Lms_backend.Domain.Entities;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IRepositoryBase<TEntity>
{
    Task<bool> ExistsAsync(Guid id, CancellationToken token);
    Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token);
    Task<bool> SaveChangesAsync(CancellationToken token);
    Task AddAsync(TEntity entity, CancellationToken token);
    void Delete(TEntity entity);
}
