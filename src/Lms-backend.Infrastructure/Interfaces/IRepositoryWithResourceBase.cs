using Lms_backend.Domain.Entities;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IRepositoryWithResourceBase<TEntity, TJoin> : IRepositoryBase<TEntity>
{
    Task<IList<Resource>> GetResourcesAsync(Guid id, CancellationToken token);
    Task<bool> AttachResourceAsync(Guid id, Guid resourceId, CancellationToken token);
    Task DetachResourceAsync(Guid id, Guid resourceId, CancellationToken token);
}
