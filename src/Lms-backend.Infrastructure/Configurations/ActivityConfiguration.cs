using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;
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
                new Activity
                {
                    Id = SeedIds.Activities.IntroToGit,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Git,
                    ActivityType = ActivityType.Lecture,
                    Name = "Introduction to Git",
                    Description = "Overview of version control concepts and setting up your first Git repository.",
                    StartTimeOffset = 0,
                    DurationMinutes = 60
                },
                new Activity
                {
                    Id = SeedIds.Activities.GitBranchingExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Git,
                    ActivityType = ActivityType.Exercise,
                    Name = "Git Branching Exercise",
                    Description = "Hands-on practice creating branches, merging changes, and resolving conflicts.",
                    StartTimeOffset = 60,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.ReadProGitBook,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Git,
                    ActivityType = ActivityType.SelfStudy,
                    Name = "Read Pro Git Book (Ch. 1-3)",
                    Description = "Self-paced reading covering Git basics, branching, and the Git workflow.",
                    StartTimeOffset = 30,
                    DurationMinutes = 120
                },
                new Activity
                {
                    Id = SeedIds.Activities.CSharpSyntax,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CSharp,
                    ActivityType = ActivityType.Lecture,
                    Name = "C# Syntax & Types",
                    Description = "Variables, data types, operators, and control flow in C#.",
                    StartTimeOffset = 0,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.OopInCSharp,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CSharp,
                    ActivityType = ActivityType.Lecture,
                    Name = "Object-Oriented Programming in C#",
                    Description = "Classes, objects, inheritance, interfaces, and encapsulation.",
                    StartTimeOffset = 90,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.OopPracticeExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CSharp,
                    ActivityType = ActivityType.Exercise,
                    Name = "OOP Practice Exercise",
                    Description = "Practice designing classes and interfaces for a small console application.",
                    StartTimeOffset = 60,
                    DurationMinutes = 120
                },
                new Activity
                {
                    Id = SeedIds.Activities.ConsoleAppAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CSharp,
                    ActivityType = ActivityType.Assignment,
                    Name = "Console App Assignment",
                    Description = "Build a small console application applying the OOP principles covered in this module.",
                    StartTimeOffset = 180,
                    DurationMinutes = 180
                },
                new Activity
                {
                    Id = SeedIds.Activities.DockerFundamentals,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Docker,
                    ActivityType = ActivityType.Lecture,
                    Name = "Docker Fundamentals",
                    Description = "Images, containers, volumes, and networking basics with Docker.",
                    StartTimeOffset = 0,
                    DurationMinutes = 60
                },
                new Activity
                {
                    Id = SeedIds.Activities.DockerfileReview,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Docker,
                    ActivityType = ActivityType.Review,
                    Name = "Dockerfile Review Session",
                    Description = "Group review and feedback on Dockerfiles written for the sample project.",
                    StartTimeOffset = 45,
                    DurationMinutes = 45
                }
                );
        }
    }
}
