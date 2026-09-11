using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.UnitTests.TestHelpers;

// Hand-rolled stand-in for IResourceRepository, kept in-memory so ResourcesService can be
// unit tested without pulling in a mocking library or a real EF context. GetResourceAsync
// reads from TrackedResources (the "write path"); GetResourceReadOnlyAsync/GetResourcesReadOnlyAsync
// read from ReadOnlyResources (or GetResourceReadOnlyHandler when set) to mirror the fact that
// the real repository re-queries a separate, untracked projection for reads.
public class FakeResourceRepository : IResourceRepository
{
    public List<Resource> TrackedResources { get; } = [];
    public List<Resource> ReadOnlyResources { get; } = [];

    public List<Resource> AddedEntities { get; } = [];
    public List<Resource> DeletedEntities { get; } = [];
    public int SaveChangesCallCount { get; private set; }
    public bool SaveChangesResult { get; set; } = true;

    public Func<Guid, Resource?>? GetResourceReadOnlyHandler { get; set; }

    public (IEnumerable<Resource> Entities, PaginationMetadata? Pagination) GetResourcesResult { get; set; } = ([], null);
    public (IEnumerable<Resource> Entities, PaginationMetadata? Pagination) GetResourcesReadOnlyResult { get; set; } = ([], null);

    public (ResourceSearchParams SearchParams, int Page, int PageSize)? LastGetResourcesReadOnlyCall { get; private set; }

    public Task<bool> ExistsAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedResources.Any(r => r.Id == id));

    public Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token) =>
        Task.FromResult<IList<Guid>>(ids.Where(id => TrackedResources.All(r => r.Id != id)).ToList());

    public Task<bool> SaveChangesAsync(CancellationToken token)
    {
        SaveChangesCallCount++;
        return Task.FromResult(SaveChangesResult);
    }

    public Task AddAsync(Resource entity, CancellationToken token)
    {
        AddedEntities.Add(entity);
        TrackedResources.Add(entity);
        return Task.CompletedTask;
    }

    public void Delete(Resource entity)
    {
        DeletedEntities.Add(entity);
        TrackedResources.Remove(entity);
    }

    public Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesAsync(ResourceSearchParams searchParams, int page, int pageSize, CancellationToken token) =>
        Task.FromResult(GetResourcesResult);

    public Task<(IEnumerable<Resource>, PaginationMetadata?)> GetResourcesReadOnlyAsync(ResourceSearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        LastGetResourcesReadOnlyCall = (searchParams, page, pageSize);
        return Task.FromResult(GetResourcesReadOnlyResult);
    }

    public Task<Resource?> GetResourceAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedResources.FirstOrDefault(r => r.Id == id));

    public Task<Resource?> GetResourceReadOnlyAsync(Guid id, CancellationToken token) =>
        Task.FromResult(GetResourceReadOnlyHandler != null
            ? GetResourceReadOnlyHandler(id)
            : ReadOnlyResources.FirstOrDefault(r => r.Id == id));
}
