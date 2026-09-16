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
                    .IsRequired()
                    .HasMaxLength(50);

            builder.Property(r => r.Description)
                    .IsRequired()
                    .HasMaxLength(200);

            builder.Property(r => r.ResourceType)
                    .IsRequired();

            builder.Property(r => r.Data)
                    .HasMaxLength(2000);

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
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowMariaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Maria,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Maria Svensson's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/maria-svensson/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowEmmaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Emma,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Emma Karlsson's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/emma-karlsson/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowOskarTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Oskar,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Oskar Lindberg's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/oskar-lindberg/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowLinaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Lina,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Lina Hakansson's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/lina-hakansson/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowJohanTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Johan,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Johan Berg's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/johan-berg/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowErikTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Erik,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Erik Holm's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/erik-holm/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowSofiaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Sofia,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Sofia Bergstrom's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/sofia-bergstrom/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.GitWorkflowAndersTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Anders,
                    Name = "Git Workflow Assignment Submission",
                    Description = "Anders Nystrom's submitted solution for the Git branching and merging workflow assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/anders-nystrom/git-workflow-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.ConsoleAppErikTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Erik,
                    Name = "Console App Assignment Submission",
                    Description = "Erik Holm's submitted solution for the console application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/erik-holm/console-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.ConsoleAppSofiaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Sofia,
                    Name = "Console App Assignment Submission",
                    Description = "Sofia Bergstrom's submitted solution for the console application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/sofia-bergstrom/console-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.ConsoleAppAndersTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Anders,
                    Name = "Console App Assignment Submission",
                    Description = "Anders Nystrom's submitted solution for the console application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/anders-nystrom/console-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.PortfolioMariaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Maria,
                    Name = "Personal Portfolio Page Submission",
                    Description = "Maria Svensson's submitted solution for the personal portfolio page assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/maria-svensson/personal-portfolio-page"
                },
                new Resource
                {
                    Id = SeedIds.Resources.PortfolioEmmaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Emma,
                    Name = "Personal Portfolio Page Submission",
                    Description = "Emma Karlsson's submitted solution for the personal portfolio page assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/emma-karlsson/personal-portfolio-page"
                },
                new Resource
                {
                    Id = SeedIds.Resources.PortfolioOskarTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Oskar,
                    Name = "Personal Portfolio Page Submission",
                    Description = "Oskar Lindberg's submitted solution for the personal portfolio page assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/oskar-lindberg/personal-portfolio-page"
                },
                new Resource
                {
                    Id = SeedIds.Resources.PortfolioLinaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Lina,
                    Name = "Personal Portfolio Page Submission",
                    Description = "Lina Hakansson's submitted solution for the personal portfolio page assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/lina-hakansson/personal-portfolio-page"
                },
                new Resource
                {
                    Id = SeedIds.Resources.TodoAppMariaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Maria,
                    Name = "Todo App Assignment Submission",
                    Description = "Maria Svensson's submitted solution for the React todo app assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/maria-svensson/todo-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.TodoAppEmmaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Emma,
                    Name = "Todo App Assignment Submission",
                    Description = "Emma Karlsson's submitted solution for the React todo app assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/emma-karlsson/todo-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.TodoAppOskarTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Oskar,
                    Name = "Todo App Assignment Submission",
                    Description = "Oskar Lindberg's submitted solution for the React todo app assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/oskar-lindberg/todo-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.TodoAppViktorTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Viktor,
                    Name = "Todo App Assignment Submission",
                    Description = "Viktor Astrom's submitted solution for the React todo app assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/viktor-astrom/todo-app-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.CrudApiJohanTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Johan,
                    Name = "CRUD API Assignment Submission",
                    Description = "Johan Berg's submitted solution for the CRUD API assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/johan-berg/crud-api-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.CrudApiErikTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Erik,
                    Name = "CRUD API Assignment Submission",
                    Description = "Erik Holm's submitted solution for the CRUD API assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/erik-holm/crud-api-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.CrudApiAndersTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Anders,
                    Name = "CRUD API Assignment Submission",
                    Description = "Anders Nystrom's submitted solution for the CRUD API assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/anders-nystrom/crud-api-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.CrudApiElinTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Elin,
                    Name = "CRUD API Assignment Submission",
                    Description = "Elin Forsberg's submitted solution for the CRUD API assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/elin-forsberg/crud-api-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DockerizeSaraTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Sara,
                    Name = "Dockerize an Application Submission",
                    Description = "Sara Lindqvist's submitted solution for the dockerize-an-application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/sara-lindqvist/dockerize-an-application"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DockerizeFredrikTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Fredrik,
                    Name = "Dockerize an Application Submission",
                    Description = "Fredrik Dahl's submitted solution for the dockerize-an-application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/fredrik-dahl/dockerize-an-application"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DockerizeNinaTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Nina,
                    Name = "Dockerize an Application Submission",
                    Description = "Nina Ekstrom's submitted solution for the dockerize-an-application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/nina-ekstrom/dockerize-an-application"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DockerizeJosefinTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Josefin,
                    Name = "Dockerize an Application Submission",
                    Description = "Josefin Lund's submitted solution for the dockerize-an-application assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/josefin-lund/dockerize-an-application"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DeployPipelineSaraTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Sara,
                    Name = "Deployment Pipeline Assignment Submission",
                    Description = "Sara Lindqvist's submitted solution for the CI/CD deployment pipeline assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/sara-lindqvist/deployment-pipeline-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DeployPipelineFredrikTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Fredrik,
                    Name = "Deployment Pipeline Assignment Submission",
                    Description = "Fredrik Dahl's submitted solution for the CI/CD deployment pipeline assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/fredrik-dahl/deployment-pipeline-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DeployPipelineJosefinTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Josefin,
                    Name = "Deployment Pipeline Assignment Submission",
                    Description = "Josefin Lund's submitted solution for the CI/CD deployment pipeline assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/josefin-lund/deployment-pipeline-assignment"
                },
                new Resource
                {
                    Id = SeedIds.Resources.DeployPipelineMartinTurnin,
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    OwnerId = SeedIds.Users.Martin,
                    Name = "Deployment Pipeline Assignment Submission",
                    Description = "Martin Oberg's submitted solution for the CI/CD deployment pipeline assignment.",
                    ResourceType = ResourceType.AssignmentTurnin,
                    Data = "https://github.com/martin-oberg/deployment-pipeline-assignment"
                }
                );
        }
    }
}
