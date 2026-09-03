using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(r => r.UpdatedAt)
                    .HasColumnType("datetime2");

            builder.Property(r => r.OwnerId)
                   .HasColumnType("int");

            builder.Property(r => r.Name)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(r => r.Description)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");

            builder.Property(r => r.ResourceType)
                    .IsRequired()
                    .HasColumnType("nvarchar");
            builder.Property(r => r.Data)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");

            builder.HasData(

                //Seed-data
                );
        }
    }
}
