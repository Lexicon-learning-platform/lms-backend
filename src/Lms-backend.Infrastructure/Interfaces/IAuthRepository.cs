using Lms_backend.Domain.Entities;

namespace Lms_backend.Infrastructure.Interfaces
{
    public interface IAuthRepository : IRepositoryBase<RefreshToken>
    {
        Task<bool> StoreRefreshTokenAsync(string refreshToken, Guid userId);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
