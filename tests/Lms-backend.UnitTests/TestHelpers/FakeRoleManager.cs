using Lms_backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lms_backend.UnitTests.TestHelpers;

// RoleManager<T> is a concrete class (not an interface), so AdminService can only be unit tested
// by subclassing it and overriding the virtual members it actually calls. See FakeUserManager for
// the same pattern; NoOpRoleStore exists purely to satisfy the base constructor.
public sealed class FakeRoleManager : RoleManager<ApplicationRole>
{
    public List<ApplicationRole> RolesList { get; } = [];

    private FakeRoleManager()
        : base(
            new NoOpRoleStore(),
            [],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            NullLogger<RoleManager<ApplicationRole>>.Instance)
    {
    }

    public static FakeRoleManager Create() => new();

    public override Task<bool> RoleExistsAsync(string roleName) =>
        Task.FromResult(RolesList.Any(r => r.Name == roleName));

    public override Task<ApplicationRole?> FindByNameAsync(string roleName) =>
        Task.FromResult(RolesList.FirstOrDefault(r => r.Name == roleName));

    private class NoOpRoleStore : IRoleStore<ApplicationRole>
    {
        public void Dispose() { }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken ct) => throw new NotSupportedException();
        public Task<string?> GetRoleNameAsync(ApplicationRole role, CancellationToken ct) => throw new NotSupportedException();
        public Task SetRoleNameAsync(ApplicationRole role, string? roleName, CancellationToken ct) => throw new NotSupportedException();
        public Task<string?> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken ct) => throw new NotSupportedException();
        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string? normalizedName, CancellationToken ct) => throw new NotSupportedException();
        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken ct) => throw new NotSupportedException();
        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken ct) => throw new NotSupportedException();
        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken ct) => throw new NotSupportedException();
        public Task<ApplicationRole?> FindByIdAsync(string roleId, CancellationToken ct) => throw new NotSupportedException();
        public Task<ApplicationRole?> FindByNameAsync(string normalizedRoleName, CancellationToken ct) => throw new NotSupportedException();
    }
}
