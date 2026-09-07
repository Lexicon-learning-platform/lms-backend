using Lms_backend.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Services
{
    public class AuthRepository(AppDbContext context) : IAuthRepository
    {
        public Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            //TODO: Implement logic to check if the refresh token is valid in the database
            throw new NotImplementedException();
        }

        public Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            //TODO: Implement logic to revoke the refresh token in the database
            //Don't delete it, just mark it as revoked
            throw new NotImplementedException();
        }

        public Task<bool> StoreRefreshTokenAsync(string refreshToken, string userName)
        {
            //TODO: Implement logic to store the refresh token in the database
            throw new NotImplementedException();
        }
    }
}
