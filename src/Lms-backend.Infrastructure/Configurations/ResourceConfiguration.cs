using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Owner)
                    .WithMany()
                    .HasForeignKey(r => r.OwnerId)
                    .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(r => r.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            builder.Property(r => r.UpdatedAt)
                    .HasDefaultValueSql("now()");

            builder.Property(r => r.Name)
                    .HasMaxLength(50);

            builder.Property(r => r.Description)
                    .HasMaxLength(200);

            builder.Property(r => r.ResourceType)
                    .IsRequired();

            builder.Property(r => r.Data)
                    .HasMaxLength(200);

            builder.HasData(
                new Resource
                {
                    Id = SeedIds.Resources.ProGitBook,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "Pro Git Book",
                    Description = "Free online book covering everything from Git basics to advanced workflows.",
                    ResourceType = ResourceType.URL,
                    Data = "https://git-scm.com/book/en/v2"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitCheatSheet,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "Git Cheat Sheet",
                    Description = "Quick reference for common Git commands.",
                    ResourceType = ResourceType.Text,
                    Data = "git init | git add . | git commit -m \"msg\" | git branch <name> | git checkout <name> | git merge <name> | git status | git log"
                },
                new Resource
                {
                    Id = SeedIds.Resources.MdnJavaScript,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "MDN Web Docs - JavaScript",
                    Description = "Comprehensive reference and guides for HTML, CSS, and JavaScript.",
                    ResourceType = ResourceType.URL,
                    Data = "https://developer.mozilla.org/en-US/docs/Web/JavaScript"
                },
                new Resource
                {
                    Id = SeedIds.Resources.CSharpConventions,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "C# Coding Conventions",
                    Description = "Notes on naming, formatting, and style conventions used in this course.",
                    ResourceType = ResourceType.Text,
                    Data = "Use PascalCase for classes/methods, camelCase for locals/params, prefix interfaces with 'I', keep methods short and single-purpose."
                },
                new Resource
                {
                    Id = SeedIds.Resources.MsLearnAspNetCore,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "Microsoft Learn - ASP.NET Core",
                    Description = "Official Microsoft documentation and tutorials for ASP.NET Core.",
                    ResourceType = ResourceType.URL,
                    Data = "https://learn.microsoft.com/aspnet/core"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DockerDocs,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "Docker Official Docs",
                    Description = "Official documentation for Docker Engine, images, and Compose.",
                    ResourceType = ResourceType.URL,
                    Data = "https://docs.docker.com/"
                },
                new Resource
                {
                    Id = SeedIds.Resources.OopPracticeInstructions,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "OOP Practice Instructions",
                    Description = "Step-by-step instructions for the object-oriented programming exercise.",
                    ResourceType = ResourceType.Text,
                    Data = "1. Define an interface IShape with an Area() method. 2. Implement Circle and Rectangle. 3. Compute the total area of a list of shapes."
                },
                new Resource
                {
                    Id = SeedIds.Resources.CourseSyllabusFullStack,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Alex,
                    Name = "Course Syllabus",
                    Description = "Full syllabus and schedule for the Full-Stack Web Development course.",
                    ResourceType = ResourceType.Text,
                    Data = "Week 1: Git. Weeks 2-4: Frontend Fundamentals. Weeks 5-8: React. Weeks 9-12: Capstone project."
                },
                new Resource
                {
                    Id = SeedIds.Resources.MariaGitNotes,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Maria,
                    Name = "My Git Notes",
                    Description = "Personal notes from the Git & Version Control module.",
                    ResourceType = ResourceType.Text,
                    Data = "Remember: commit early and often. Use feature branches. Run 'git status' before every commit. Ask Alex about rebase vs merge."
                },
                new Resource
                {
                    Id = SeedIds.Resources.JohanConsoleAppTurnIn,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Johan,
                    Name = "Console App Assignment Submission",
                    Description = "Johan Berg's submitted solution for the console application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/johan-berg/console-app-assignment"
                }
                );
        }
    }
}
