using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Infrastructure.Interfaces
{
    public interface IAuthRepository : IRepositoryBase<RefreshToken>
    {
        Task<bool> StoreRefreshTokenAsync(string refreshToken, Guid userId);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        Task<ActionResponse> RevokeRefreshTokenAsync(string refreshToken);
    }
}
