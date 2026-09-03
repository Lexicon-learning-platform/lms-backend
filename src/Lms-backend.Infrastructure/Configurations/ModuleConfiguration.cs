using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            builder.Property(m => m.UpdatedAt)
                    .HasDefaultValueSql("now()");

            builder.Property(m => m.Name)
                    .HasMaxLength(50);

            builder.Property(m => m.Description)
                    .HasMaxLength(200);

            builder.HasData(
                new Module
                {
                    Id = SeedIds.Modules.Git,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "Git & Version Control",
                    Description = "Version control fundamentals: repositories, commits, branching, merging, and collaborative workflows.",
                    Duration = 1
                },
                new Module
                {
                    Id = SeedIds.Modules.Frontend,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "Frontend Fundamentals",
                    Description = "Core building blocks of the web: semantic HTML, CSS layout & styling, and JavaScript basics.",
                    Duration = 3
                },
                new Module
                {
                    Id = SeedIds.Modules.React,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "React Fundamentals",
                    Description = "Building interactive user interfaces with components, props, state, and hooks in React.",
                    Duration = 3
                },
                new Module
                {
                    Id = SeedIds.Modules.CSharp,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "C# Language Fundamentals",
                    Description = "Core C# syntax, types, control flow, and object-oriented programming concepts.",
                    Duration = 3
                },
                new Module
                {
                    Id = SeedIds.Modules.AspNetCore,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "ASP.NET Core & EF Core",
                    Description = "Building REST APIs with ASP.NET Core and persisting data with Entity Framework Core.",
                    Duration = 4
                },
                new Module
                {
                    Id = SeedIds.Modules.Docker,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "Containers with Docker",
                    Description = "Packaging and running applications in containers using Docker images and Compose.",
                    Duration = 2
                },
                new Module
                {
                    Id = SeedIds.Modules.CiCd,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    Name = "CI/CD & Cloud Deployment",
                    Description = "Automating build, test, and deployment pipelines and deploying to cloud platforms.",
                    Duration = 3
                }
                            );
        }
    }
}
