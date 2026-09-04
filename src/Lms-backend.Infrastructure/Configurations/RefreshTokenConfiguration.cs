using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lms_backend.Infrastructure.Configurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {

        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasOne(t => t.User)
                    .WithMany()
                    .HasForeignKey(t => t.UserId);

            builder.Property(t => t.UserId)
                    .IsRequired();

            builder.Property(t => t.TokenHash)
                    .IsRequired();

            builder.Property(t => t.ExpiresAt)
                    .IsRequired();

            builder.Property(t => t.RevokedAt);

            builder.Property(t => t.DeviceInfo)
                    .HasMaxLength(50);

            builder.Property(t => t.LastUsedAt);

        }
    }
}