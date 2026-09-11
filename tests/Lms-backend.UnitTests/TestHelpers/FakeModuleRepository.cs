using Lms_backend.Domain.Entities;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.UnitTests.TestHelpers;

// Hand-rolled stand-in for IModuleRepository, mirroring FakeResourceRepository so
// ModulesService (and CoursesService, which depends on it for module-existence checks)
// can be unit tested without a mocking library or a real EF context. GetModuleAsync reads
// from TrackedModules (the "write path"); GetModuleReadOnlyAsync/GetModulesReadOnlyAsync
// read from ReadOnlyModules, mirroring the real repository's separate untracked projection.
public class FakeModuleRepository : IModuleRepository
{
    public List<Module> TrackedModules { get; } = [];
    public List<Module> ReadOnlyModules { get; } = [];

    public List<Module> AddedEntities { get; } = [];
    public List<Module> DeletedEntities { get; } = [];
    public int SaveChangesCallCount { get; private set; }
    public bool SaveChangesResult { get; set; } = true;

    public Dictionary<Guid, IList<Resource>> ResourcesByModuleId { get; } = [];
    public bool AttachResourceResult { get; set; } = true;
    public List<(Guid ModuleId, Guid ResourceId)> AttachResourceCalls { get; } = [];
    public List<(Guid ModuleId, Guid ResourceId)> DetachResourceCalls { get; } = [];

    public (IEnumerable<Module> Entities, PaginationMetadata? Pagination) GetModulesResult { get; set; } = ([], null);
    public (IEnumerable<Module> Entities, PaginationMetadata? Pagination) GetModulesReadOnlyResult { get; set; } = ([], null);
    public (ModuleSearchParams SearchParams, int Page, int PageSize)? LastGetModulesReadOnlyCall { get; private set; }

    public IList<Guid> MissingIds { get; set; } = [];
    public ICollection<Guid>? LastGetMissingIdsCall { get; private set; }

    public Task<bool> ExistsAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedModules.Any(m => m.Id == id));

    public Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token)
    {
        LastGetMissingIdsCall = ids;
        return Task.FromResult(MissingIds);
    }

    public Task<bool> SaveChangesAsync(CancellationToken token)
    {
        SaveChangesCallCount++;
        return Task.FromResult(SaveChangesResult);
    }

    public Task AddAsync(Module entity, CancellationToken token)
    {
        AddedEntities.Add(entity);
        TrackedModules.Add(entity);
        return Task.CompletedTask;
    }

    public void Delete(Module entity)
    {
        DeletedEntities.Add(entity);
        TrackedModules.Remove(entity);
    }

    public Task<IList<Resource>> GetResourcesAsync(Guid id, CancellationToken token) =>
        Task.FromResult(ResourcesByModuleId.TryGetValue(id, out var resources) ? resources : (IList<Resource>)[]);

    public Task<bool> AttachResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        AttachResourceCalls.Add((id, resourceId));
        return Task.FromResult(AttachResourceResult);
    }

    public Task DetachResourceAsync(Guid id, Guid resourceId, CancellationToken token)
    {
        DetachResourceCalls.Add((id, resourceId));
        return Task.CompletedTask;
    }

    public Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesAsync(ModuleSearchParams searchParams, int page, int pageSize, CancellationToken token) =>
        Task.FromResult(GetModulesResult);

    public Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesReadOnlyAsync(ModuleSearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        LastGetModulesReadOnlyCall = (searchParams, page, pageSize);
        return Task.FromResult(GetModulesReadOnlyResult);
    }

    public Task<Module?> GetModuleAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedModules.FirstOrDefault(m => m.Id == id));

    public Task<Module?> GetModuleReadOnlyAsync(Guid id, CancellationToken token) =>
        Task.FromResult(ReadOnlyModules.FirstOrDefault(m => m.Id == id) ?? TrackedModules.FirstOrDefault(m => m.Id == id));
}
