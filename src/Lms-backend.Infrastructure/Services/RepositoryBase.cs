using Lms_backend.Domain.Interfaces;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public abstract class RepositoryBase<TEntity, TResource>(AppDbContext context) : IRepositoryBase<TEntity, TResource> where TEntity : class, IEntity where TResource : class, IEntity
{
    protected AppDbContext Context { get; } = context;
    protected abstract DbSet<TEntity> Set { get; }
    protected abstract DbSet<TResource> ResourceSet { get; }

    public async Task AddAsync(TEntity entity, CancellationToken token)
    {
        await Set.AddAsync(entity, token);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken token)
    {
        return Set.AnyAsync(e => e.Id == id, token);
    }

    public async Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token)
    {
        var found = await Set.Where(e => ids.Contains(e.Id)).Select(e => e.Id).ToListAsync(token);
        return [.. ids.Except(found)];
    }

    public async Task<bool> SaveChangesAsync(TEntity entity, CancellationToken token)
    {
        return await Context.SaveChangesAsync(token) > 0;
    }
}
