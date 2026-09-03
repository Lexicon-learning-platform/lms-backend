using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {

            builder.HasOne<Module>(a => a.Modules)
                    .WithMany(m => m.Activities)
                    .HasForeignKey(a => a.ModuleId);

            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            builder.Property(a => a.UpdatedAt)
                    .HasDefaultValueSql("now()");

            builder.Property(a => a.ActivityType)
                    .IsRequired();

            builder.Property(a => a.Name)
                    .HasMaxLength(50);

            builder.Property(a => a.Description)
                    .HasMaxLength(200);

            builder.Property(a => a.StartTimeOffset)
                    .IsRequired();

            builder.Property(a => a.DurationMinutes)
                    .IsRequired();

            builder.HasData(

                //Seed-data
                );
        }
    }
}
