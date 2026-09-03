using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.ResourceId);

            builder.HasOne(r => r.Owner)
                    .WithMany()
                    .HasForeignKey(r => r.OwnerId)
                    .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(r => r.CreatedAt)
                    .IsRequired();

            builder.Property(r => r.Name)
                    .HasMaxLength(50);

            builder.Property(r => r.Description)
                    .HasMaxLength(200);

            builder.Property(r => r.ResourceType)
                    .IsRequired();

            builder.Property(r => r.Data)
                    .HasMaxLength(200);

            builder.HasData(

                //Seed-data
                );
        }
    }
}
