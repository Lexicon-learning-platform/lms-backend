using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne<Course>(u => u.Course)
                    .WithMany(c => c.Users)
                    .HasForeignKey(u => u.CourseId);

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            builder.Property(u => u.UpdatedAt)
                    .HasDefaultValueSql("now()");

            builder.Property(u => u.GivenName)
                    .HasMaxLength(50);

            builder.Property(u => u.LastName)
                    .HasMaxLength(50);

            builder.HasData(

                //Seed-data
                );
        }
    }
}
