using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
using Lms_backend.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Services
{
    public class AuthRepository(AppDbContext context) : RepositoryBase<RefreshToken>(context), IAuthRepository
    {
        protected override DbSet<RefreshToken> Set => Context.RefreshTokens;

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            var query = Set.AsNoTracking().Where(rt => rt.TokenHash == refreshToken && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow);

            return await query.AnyAsync();
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var query = Set.Where(rt => rt.TokenHash == refreshToken && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow);

            var token = await query.FirstOrDefaultAsync();

            if (token == null)
                return false;

            token.RevokedAt = DateTime.UtcNow;
            Context.Update(token);
            await Context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> StoreRefreshTokenAsync(string refreshToken, Guid userId)
        {
            var token = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                DeviceInfo = "Device Info",
                LastUsedAt = DateTime.UtcNow
            };

            Context.RefreshTokens.Add(token);
            await Context.SaveChangesAsync();

            return true;
        }
    }
}
