using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Domain.Entities
{
    public record RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = default!;

        public string TokenHash { get; set; } = string.Empty;

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? DeviceInfo { get; set; }

        public DateTime? LastUsedAt { get; set; }

    }
}
