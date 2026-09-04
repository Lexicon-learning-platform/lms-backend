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

            builder.Property(t => t.UserId)
                    .IsRequired()
                    .HasColumnType("int");

            builder.Property(t => t.TokenHash)
                    .IsRequired()
                    .HasColumnType("nvarchar");
            
            builder.Property(t => t.ExpiresAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(t => t.RevokedAt)
                    .HasColumnType("datetime2");

            builder.Property(t => t.DeviceInfo)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(t => t.LastUsedAt)
                    .HasColumnType("datetime2");

        }
    }
}