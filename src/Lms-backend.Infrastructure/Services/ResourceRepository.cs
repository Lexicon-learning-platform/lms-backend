using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class ResourceRepository(AppDbContext context) : RepositoryBase<Resource>(context), IResourceRepository
{
    protected override DbSet<Resource> Set => Context.Resources;

    public Task<Resource?> GetResourceAsync(Guid id, CancellationToken token)
    {
        return GetResourceInternalAsync(id, false, token);
    }

    public Task<Resource?> GetResourceReadOnlyAsync(Guid id, CancellationToken token)
    {
        return GetResourceInternalAsync(id, true, token);
    }

    private async Task<Resource?> GetResourceInternalAsync(Guid id, bool readOnly, CancellationToken token)
    {
        var query = Set.AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(r => r.Id == id, token);
    }

    public Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesAsync(ResourceSearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetResourcesInternalAsync(searchParams, page, pageSize, false, token);
    }

    public Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesReadOnlyAsync(ResourceSearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetResourcesInternalAsync(searchParams, page, pageSize, true, token);
    }

    private async Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesInternalAsync(ResourceSearchParams searchParams, int page, int pageSize, bool readOnly, CancellationToken token)
    {
        var query = Set.AsSplitQuery().AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchParams.Name)) query = query.Where(a => a.Name.Contains(searchParams.Name));
        if (!string.IsNullOrWhiteSpace(searchParams.Search)) query = query.Where(a => a.Name.Contains(searchParams.Search) || a.Description.Contains(searchParams.Search));
        if (searchParams.Type.HasValue) query = query.Where(a => a.ResourceType == searchParams.Type.Value);

        var totalCount = await query.CountAsync(token);
        var pagination = new PaginationMetadata(totalCount, pageSize, page);

        var activities = await query
            .OrderBy(r => r.CreatedAt).ThenBy(r => r.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return (activities, pagination);
    }
}
