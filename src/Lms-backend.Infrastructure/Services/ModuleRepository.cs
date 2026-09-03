using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Interfaces;
using Lms_backend.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Lms_backend.Infrastructure.Services;

public class ModuleRepository(AppDbContext context) : RepositoryWithResourceBase<Module, ModuleResource>(context), IModuleRepository
{
    protected override DbSet<Module> Set => Context.Modules;
    protected override DbSet<ModuleResource> JoinSet => Context.ModuleResources;

    protected override IQueryable<ModuleResource> JoinsForEntity(Guid entityId) =>
        JoinSet.Where(j => j.ModuleId == entityId);

    protected override ModuleResource CreateJoin(Guid entityId, Guid resourceId) =>
        new() { ModuleId = entityId, ResourceId = resourceId };

    public Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesAsync(ModuleSearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetModulesInternalAsync(searchParams, page, pageSize, false, token);
    }

    public Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesReadOnlyAsync(ModuleSearchParams searchParams, int page, int pageSize, CancellationToken token)
    {
        return GetModulesInternalAsync(searchParams, page, pageSize, true, token);
    }

    private async Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesInternalAsync(ModuleSearchParams searchParams, int page, int pageSize, bool readOnly, CancellationToken token)
    {
        var query = Set.AsSplitQuery().AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchParams.Name)) query = query.Where(m => m.Name.Contains(searchParams.Name));
        if (!string.IsNullOrWhiteSpace(searchParams.Search)) query = query.Where(m => m.Name.Contains(searchParams.Search) || m.Description.Contains(searchParams.Search));
        if (searchParams.CourseId.HasValue) query = query.Where(m => m.Courses.Any(cm => cm.CourseId == searchParams.CourseId.Value));

        var totalCount = await query.CountAsync(token);
        var pagination = new PaginationMetadata(totalCount, pageSize, page);

        var courses = await query
            .OrderBy(c => c.Name)
            .Include(c => c.Activities)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return (courses, pagination);
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
        var query = Set
            .Include(c => c.Activities)
            .Include(c => c.Resources).ThenInclude(cr => cr.Resource)
            .AsSplitQuery()
            .AsQueryable();

        if (readOnly) query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(c => c.Id == id, token);
    }
}
