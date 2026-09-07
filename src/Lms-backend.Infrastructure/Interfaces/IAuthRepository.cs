using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> StoreRefreshTokenAsync(string refreshToken, string userName);
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
