using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Interfaces
{
    public interface IAuthRepository : IRepositoryBase<RefreshToken>
    {
        Task<bool> StoreRefreshTokenAsync(string refreshToken, Guid userId);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
