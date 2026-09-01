using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IResourceRepository : IRepositoryBase<Resource>
{
    Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<Resource?> GetResourceAsync(Guid id, CancellationToken token);
    Task<Resource?> GetResourceReadOnlyAsync(Guid id, CancellationToken token);
}
