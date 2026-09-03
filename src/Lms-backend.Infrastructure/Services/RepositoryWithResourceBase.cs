using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public abstract class RepositoryWithResourceBase<TEntity, TJoin>(AppDbContext context) : RepositoryBase<TEntity>(context), IRepositoryWithResourceBase<TEntity, TJoin> where TEntity : class, IEntity where TJoin : class, IResourceJoin
{
    protected abstract DbSet<TJoin> JoinSet { get; }

    protected abstract IQueryable<TJoin> JoinsForEntity(Guid entityId);

    protected abstract TJoin CreateJoin(Guid entityId, Guid resourceId);

    public async Task<IList<Resource>> GetResourcesAsync(Guid id, CancellationToken token)
    {
        return await JoinsForEntity(id)
            .Select(j => j.Resource!)
            .ToListAsync(token);
    }

    public async Task<bool> AttachResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        var alreadyAttached = await JoinsForEntity(id).AnyAsync(j => j.ResourceId == resourceId, token);
        if (alreadyAttached) return false;

        await JoinSet.AddAsync(CreateJoin(id, resourceId), token);
        return true;
    }

    public async Task DetachResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        var join = await JoinsForEntity(id).FirstOrDefaultAsync(j => j.ResourceId == resourceId, token);
        if (join is not null) JoinSet.Remove(join);
    }
}
