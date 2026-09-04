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
                new ApplicationUser
                {
                    Id = SeedIds.Users.Alex,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Alex",
                    LastName = "Nilsson",
                    CourseId = null
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Maria,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Maria",
                    LastName = "Svensson",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Johan,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Johan",
                    LastName = "Berg",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Sara,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Sara",
                    LastName = "Lindqvist",
                    CourseId = SeedIds.Courses.CloudDevOps
                }
                );
        }
    }
}
