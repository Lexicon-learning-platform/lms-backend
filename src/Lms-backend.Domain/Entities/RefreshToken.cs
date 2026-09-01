using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Domain.Entities
{
    public record RefreshToken
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

        public HashCode TokenHash { get; set; } = new HashCode();

        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public string? DeviceInfo { get; set; }

        public DateTime? LastUsedAt { get; set; }

    }
}
