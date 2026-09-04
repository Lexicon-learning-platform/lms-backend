using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            builder.Property(c => c.UpdatedAt)
                    .HasDefaultValueSql("now()");

            builder.Property(c => c.Name)
                    .HasMaxLength(50);

            builder.Property(c => c.Description)
                    .HasMaxLength(200);

            builder.HasData(
                new Course
                {
                    Id = SeedIds.Courses.FullStack,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "Full-Stack Web Development",
                    Description = "Learn to build modern web applications end-to-end, from responsive front-ends to REST APIs and databases.",
                    StartDate = new DateOnly(2026, 2, 2),
                    Duration = 12
                },
                new Course
                {
                    Id = SeedIds.Courses.Backend,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "Backend Development with C# & .NET",
                    Description = "Deep dive into C#, object-oriented design, and building robust web APIs with ASP.NET Core and EF Core.",
                    StartDate = new DateOnly(2026, 2, 2),
                    Duration = 10
                },
                new Course
                {
                    Id = SeedIds.Courses.CloudDevOps,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "Cloud & DevOps Engineering",
                    Description = "Containerize, automate, and deploy applications using Docker, CI/CD pipelines, and cloud platforms.",
                    StartDate = new DateOnly(2026, 3, 2),
                    Duration = 8
                }
                );
        }
    }
}
