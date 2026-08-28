using Lms_backend.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.ActivityId);

            builder.Property(a => a.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(a => a.UpdatedAt)
                    .HasColumnType("datetime2");

            builder.Property(a => a.ModuleId)
                    .HasColumnType("int");

            builder.Property(a => a.ActivityType)
                    .IsRequired()
                    .HasColumnType("int");

            builder.Property(a => a.Name)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(a => a.Description)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");

            builder.Property(a => a.StartTimeOffset)
                    .IsRequired()
                    .HasColumnType("int");

            builder.Property(a => a.DurationMinutes)
                    .IsRequired()
                    .HasColumnType("int");

            builder.HasData(

                //Seed-data
                );
        }
    }
}
