using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Models;

namespace Lms_backend.Infrastructure.Interfaces;

public interface IModuleRepository : IRepositoryWithResourceBase<Module, ModuleResource>
{
    Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesAsync(ModuleSearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<(IEnumerable<Module>, PaginationMetadata?)> GetModulesReadOnlyAsync(ModuleSearchParams searchParams, int page, int pageSize, CancellationToken token);
    Task<Module?> GetModuleAsync(Guid id, CancellationToken token);
    Task<Module?> GetModuleReadOnlyAsync(Guid id, CancellationToken token);
}
