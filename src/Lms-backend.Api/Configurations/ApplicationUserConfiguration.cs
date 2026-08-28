using Lms_backend.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne<Course>(u => u.Course)
                    .WithMany(c => c.ApplicationUsers)
                    .HasForeignKey(u => u.CourseId);

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(u => u.UpdatedAt)
                    .HasColumnType("datetime2");

            builder.Property(u => u.GivenName)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(u => u.LastName)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(u => u.CourseId)
                    .HasColumnType("int");

            builder.HasData(

                //Seed-data
                );
        }
    }
}
