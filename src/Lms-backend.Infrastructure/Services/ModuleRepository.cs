using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class ModuleRepository(AppDbContext context) : RepositoryBase<Module, ModuleResource>(context), IModuleRepository
{
    protected override DbSet<Module> Set => Context.Modules;
    protected override DbSet<ModuleResource> JoinSet => Context.ModuleResources;

    public Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetModulesInternalAsync(searchParams, page, pageSize, false, token);
    }

    public Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetModulesInternalAsync(searchParams, page, pageSize, true, token);
    }

    private async Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesInternalAsync(SearchParams searchParams, int page, int pageSize, bool readOnly, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<Module?> GetModuleAsync(Guid id, CancellationToken token)
    {
        return GetModuleInternalAsync(id, false, token);
    }

    public Task<Module?> GetModuleReadOnlyAsync(Guid id, CancellationToken token)
    {
        return GetModuleInternalAsync(id, true, token);
    }

    private async Task<Module?> GetModuleInternalAsync(Guid id, bool readOnly, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
