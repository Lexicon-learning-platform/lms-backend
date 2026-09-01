using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryBase<Module, ModuleResource>
{
    Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesReadOnlyAsync(SearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<Module?> GetModuleAsync(Guid id, CancellationToken token);
    Task<Module?> GetModuleReadOnlyAsync(Guid id, CancellationToken token);
}
