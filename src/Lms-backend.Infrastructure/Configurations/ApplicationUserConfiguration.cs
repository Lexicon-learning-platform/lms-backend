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
                    ConcurrencyStamp = "268d2cf5-1946-4e8e-b915-7f66ea6abed8",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Alex",
                    LastName = "Nilsson",
                    CourseId = null
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Maria,
                    ConcurrencyStamp = "01f429a1-8204-4597-bc2a-e763ee8e1e9b",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Maria",
                    LastName = "Svensson",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Johan,
                    ConcurrencyStamp = "48fd668d-3a3b-40d5-9c9e-e96de93a4458",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Johan",
                    LastName = "Berg",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Sara,
                    ConcurrencyStamp = "4a08b3ba-e848-4ca6-aee6-51fa69ded960",
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
