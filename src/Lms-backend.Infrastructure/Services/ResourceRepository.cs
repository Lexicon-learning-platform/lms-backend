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
        throw new NotImplementedException();
    }

    public Task<Resource?> GetResourceReadOnlyAsync(Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
