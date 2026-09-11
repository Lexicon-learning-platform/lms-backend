using Lms_backend.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace Lms_backend.UnitTests.TestHelpers;

// UserManager<T> is a concrete class (not an interface), so AuthService/AdminService can only be
// unit tested by subclassing it and overriding the virtual members they actually call. The base
// constructor still needs a store/hasher/etc., but since every method used by those services is
// overridden below, the store is never touched - NoOpUserStore exists purely to satisfy the
// constructor signature.
public sealed class FakeUserManager : UserManager<ApplicationUser>
{
    public List<ApplicationUser> UsersList { get; } = [];

    public Func<ApplicationUser, string, bool>? CheckPasswordHandler { get; set; }

    public List<ApplicationUser> CreatedUsers { get; } = [];
    public List<ApplicationUser> DeletedUsers { get; } = [];
    public List<ApplicationUser> UpdatedUsers { get; } = [];
    public List<(ApplicationUser User, string Password)> AddPasswordCalls { get; } = [];
    public List<(ApplicationUser User, string CurrentPassword, string NewPassword)> ChangePasswordCalls { get; } = [];
    public List<(ApplicationUser User, DateTimeOffset? LockoutEnd)> SetLockoutEndDateCalls { get; } = [];

    private FakeUserManager()
        : base(
            new NoOpUserStore(),
            Microsoft.Extensions.Options.Options.Create(new IdentityOptions()),
            new PasswordHasher<ApplicationUser>(),
            [],
            [],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            NullLogger<UserManager<ApplicationUser>>.Instance)
    {
    }

    public static FakeUserManager Create() => new();

    public override IQueryable<ApplicationUser> Users => UsersList.AsQueryable();

    public override Task<ApplicationUser?> FindByIdAsync(string userId) =>
        Task.FromResult(UsersList.FirstOrDefault(u => u.Id.ToString() == userId));

    public override Task<ApplicationUser?> FindByNameAsync(string userName) =>
        Task.FromResult(UsersList.FirstOrDefault(u => u.UserName == userName));

    public override Task<bool> CheckPasswordAsync(ApplicationUser user, string password) =>
        Task.FromResult(CheckPasswordHandler?.Invoke(user, password) ?? false);

    public override Task<IdentityResult> CreateAsync(ApplicationUser user)
    {
        CreatedUsers.Add(user);
        UsersList.Add(user);
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> DeleteAsync(ApplicationUser user)
    {
        DeletedUsers.Add(user);
        UsersList.Remove(user);
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> UpdateAsync(ApplicationUser user)
    {
        UpdatedUsers.Add(user);
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> AddPasswordAsync(ApplicationUser user, string password)
    {
        AddPasswordCalls.Add((user, password));
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        ChangePasswordCalls.Add((user, currentPassword, newPassword));
        return Task.FromResult(IdentityResult.Success);
    }

    public override Task<IdentityResult> SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset? lockoutEnd)
    {
        SetLockoutEndDateCalls.Add((user, lockoutEnd));
        return Task.FromResult(IdentityResult.Success);
    }

    private class NoOpUserStore : IUserStore<ApplicationUser>
    {
        public void Dispose() { }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken ct) => throw new NotSupportedException();
        public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken ct) => throw new NotSupportedException();
        public Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken ct) => throw new NotSupportedException();
        public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken ct) => throw new NotSupportedException();
        public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken ct) => throw new NotSupportedException();
        public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken ct) => throw new NotSupportedException();
        public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken ct) => throw new NotSupportedException();
        public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken ct) => throw new NotSupportedException();
        public Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken ct) => throw new NotSupportedException();
        public Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken ct) => throw new NotSupportedException();
    }
}
