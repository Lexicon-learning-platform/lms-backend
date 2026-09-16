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
                },
                new Activity
                {
                    Id = SeedIds.Activities.GitWorkflowAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Git,
                    ActivityType = ActivityType.Assignment,
                    Name = "Git Workflow Assignment",
                    Description = "Apply branching, merging, and commit conventions to complete a guided multi-branch Git workflow.",
                    StartTimeOffset = 150,
                    DurationMinutes = 120
                },
                new Activity
                {
                    Id = SeedIds.Activities.HtmlCssFundamentals,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Frontend,
                    ActivityType = ActivityType.Lecture,
                    Name = "HTML & CSS Fundamentals",
                    Description = "Semantic HTML structure and the CSS box model, Flexbox, and Grid layout basics.",
                    StartTimeOffset = 0,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.ResponsiveLayoutExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Frontend,
                    ActivityType = ActivityType.Exercise,
                    Name = "Responsive Layout Exercise",
                    Description = "Hands-on practice building a mobile-first, responsive page layout with Flexbox and media queries.",
                    StartTimeOffset = 90,
                    DurationMinutes = 120
                },
                new Activity
                {
                    Id = SeedIds.Activities.PortfolioPageAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Frontend,
                    ActivityType = ActivityType.Assignment,
                    Name = "Personal Portfolio Page",
                    Description = "Build and publish a responsive personal portfolio page using semantic HTML and CSS.",
                    StartTimeOffset = 210,
                    DurationMinutes = 240
                },
                new Activity
                {
                    Id = SeedIds.Activities.ReactComponentsProps,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.React,
                    ActivityType = ActivityType.Lecture,
                    Name = "React Components & Props",
                    Description = "Building reusable UI components and passing data between them with props.",
                    StartTimeOffset = 0,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.StateHooksExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.React,
                    ActivityType = ActivityType.Exercise,
                    Name = "State & Hooks Exercise",
                    Description = "Hands-on practice managing component state and side effects with useState and useEffect.",
                    StartTimeOffset = 90,
                    DurationMinutes = 120
                },
                new Activity
                {
                    Id = SeedIds.Activities.TodoAppAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.React,
                    ActivityType = ActivityType.Assignment,
                    Name = "Todo App Assignment",
                    Description = "Build a functional todo list application using React components, state, and hooks.",
                    StartTimeOffset = 210,
                    DurationMinutes = 240
                },
                new Activity
                {
                    Id = SeedIds.Activities.BuildingRestApis,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.AspNetCore,
                    ActivityType = ActivityType.Lecture,
                    Name = "Building REST APIs",
                    Description = "Designing and implementing RESTful endpoints with ASP.NET Core controllers and routing.",
                    StartTimeOffset = 0,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.EfCoreMigrationsExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.AspNetCore,
                    ActivityType = ActivityType.Exercise,
                    Name = "EF Core Migrations Exercise",
                    Description = "Hands-on practice modeling entities and managing schema changes with EF Core migrations.",
                    StartTimeOffset = 90,
                    DurationMinutes = 120
                },
                new Activity
                {
                    Id = SeedIds.Activities.CrudApiAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.AspNetCore,
                    ActivityType = ActivityType.Assignment,
                    Name = "CRUD API Assignment",
                    Description = "Build a complete CRUD REST API with ASP.NET Core and EF Core backed by PostgreSQL.",
                    StartTimeOffset = 210,
                    DurationMinutes = 240
                },
                new Activity
                {
                    Id = SeedIds.Activities.CustomDockerImageExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Docker,
                    ActivityType = ActivityType.Exercise,
                    Name = "Build a Custom Image",
                    Description = "Hands-on practice writing a Dockerfile and building a custom image for a sample application.",
                    StartTimeOffset = 90,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.DockerizeAppAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.Docker,
                    ActivityType = ActivityType.Assignment,
                    Name = "Dockerize an Application",
                    Description = "Containerize a sample application with a Dockerfile and a Docker Compose setup.",
                    StartTimeOffset = 180,
                    DurationMinutes = 180
                },
                new Activity
                {
                    Id = SeedIds.Activities.CiCdPipelineConcepts,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CiCd,
                    ActivityType = ActivityType.Lecture,
                    Name = "CI/CD Pipeline Concepts",
                    Description = "Overview of continuous integration and deployment concepts, stages, and common tooling.",
                    StartTimeOffset = 0,
                    DurationMinutes = 60
                },
                new Activity
                {
                    Id = SeedIds.Activities.GithubActionsExercise,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CiCd,
                    ActivityType = ActivityType.Exercise,
                    Name = "GitHub Actions Workflow",
                    Description = "Hands-on practice writing a GitHub Actions workflow to build and test an application.",
                    StartTimeOffset = 60,
                    DurationMinutes = 90
                },
                new Activity
                {
                    Id = SeedIds.Activities.DeploymentPipelineAssignment,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    ModuleId = SeedIds.Modules.CiCd,
                    ActivityType = ActivityType.Assignment,
                    Name = "Deployment Pipeline Assignment",
                    Description = "Build an automated CI/CD pipeline that builds, tests, and deploys a sample application.",
                    StartTimeOffset = 150,
                    DurationMinutes = 180
                }
                );
        }
    }
}
