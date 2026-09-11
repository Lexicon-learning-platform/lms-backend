using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Lms_backend.Infrastructure.Interfaces;

namespace Lms_backend.UnitTests.TestHelpers;

// Hand-rolled stand-in for IAuthRepository so AuthService can be unit tested without a mocking
// library or a real EF context.
public class FakeAuthRepository : IAuthRepository
{
    public List<RefreshToken> TrackedTokens { get; } = [];

    public bool StoreRefreshTokenResult { get; set; } = true;
    public List<(string Token, Guid UserId)> StoreRefreshTokenCalls { get; } = [];

    public bool IsRefreshTokenValidResult { get; set; } = true;
    public List<string> IsRefreshTokenValidCalls { get; } = [];

    public ActionResponse RevokeRefreshTokenResult { get; set; } = ActionResponse.Success;
    public List<string> RevokeRefreshTokenCalls { get; } = [];

    public Task<bool> ExistsAsync(Guid id, CancellationToken token) =>
        Task.FromResult(TrackedTokens.Any(t => t.Id == id));

    public Task<IList<Guid>> GetMissingIdsAsync(ICollection<Guid> ids, CancellationToken token) =>
        Task.FromResult<IList<Guid>>(ids.Where(id => TrackedTokens.All(t => t.Id != id)).ToList());

    public Task<bool> SaveChangesAsync(CancellationToken token) => Task.FromResult(true);

    public Task AddAsync(RefreshToken entity, CancellationToken token)
    {
        TrackedTokens.Add(entity);
        return Task.CompletedTask;
    }

    public void Delete(RefreshToken entity) => TrackedTokens.Remove(entity);

    public Task<bool> StoreRefreshTokenAsync(string refreshToken, Guid userId)
    {
        StoreRefreshTokenCalls.Add((refreshToken, userId));
        return Task.FromResult(StoreRefreshTokenResult);
    }

    public Task<bool> IsRefreshTokenValidAsync(string refreshToken)
    {
        IsRefreshTokenValidCalls.Add(refreshToken);
        return Task.FromResult(IsRefreshTokenValidResult);
    }

    public Task<ActionResponse> RevokeRefreshTokenAsync(string refreshToken)
    {
        RevokeRefreshTokenCalls.Add(refreshToken);
        return Task.FromResult(RevokeRefreshTokenResult);
    }
}
