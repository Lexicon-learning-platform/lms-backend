using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations.Joins
{
    public class ActivityResourceConfiguration : IEntityTypeConfiguration<ActivityResource>
    {
        public void Configure(EntityTypeBuilder<ActivityResource> builder)
        {
            builder.HasOne(ar => ar.Activity)
        .WithMany(a => a.Resources)
        .HasForeignKey(ar => ar.ActivityId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(mr => mr.Resource)
        .WithMany()
        .HasForeignKey(mr => mr.ResourceId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);

            builder.HasIndex(j => new { j.ActivityId, j.ResourceId })
                    .IsUnique();

            builder.HasData(
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.IntroToGitProGitBook,
                    ActivityId = SeedIds.Activities.IntroToGit,
                    ResourceId = SeedIds.Resources.ProGitBook
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitBranchingExerciseCheatSheet,
                    ActivityId = SeedIds.Activities.GitBranchingExercise,
                    ResourceId = SeedIds.Resources.GitCheatSheet
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.OopPracticeExerciseInstructions,
                    ActivityId = SeedIds.Activities.OopPracticeExercise,
                    ResourceId = SeedIds.Resources.OopPracticeInstructions
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.ConsoleAppAssignmentTurnIn,
                    ActivityId = SeedIds.Activities.ConsoleAppAssignment,
                    ResourceId = SeedIds.Resources.JohanConsoleAppTurnIn
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowMariaTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowMariaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowEmmaTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowEmmaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowOskarTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowOskarTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowLinaTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowLinaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowJohanTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowJohanTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowErikTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowErikTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowSofiaTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowSofiaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.GitWorkflowAndersTurnin,
                    ActivityId = SeedIds.Activities.GitWorkflowAssignment,
                    ResourceId = SeedIds.Resources.GitWorkflowAndersTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.ConsoleAppErikTurnin,
                    ActivityId = SeedIds.Activities.ConsoleAppAssignment,
                    ResourceId = SeedIds.Resources.ConsoleAppErikTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.ConsoleAppSofiaTurnin,
                    ActivityId = SeedIds.Activities.ConsoleAppAssignment,
                    ResourceId = SeedIds.Resources.ConsoleAppSofiaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.ConsoleAppAndersTurnin,
                    ActivityId = SeedIds.Activities.ConsoleAppAssignment,
                    ResourceId = SeedIds.Resources.ConsoleAppAndersTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.PortfolioMariaTurnin,
                    ActivityId = SeedIds.Activities.PortfolioPageAssignment,
                    ResourceId = SeedIds.Resources.PortfolioMariaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.PortfolioEmmaTurnin,
                    ActivityId = SeedIds.Activities.PortfolioPageAssignment,
                    ResourceId = SeedIds.Resources.PortfolioEmmaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.PortfolioOskarTurnin,
                    ActivityId = SeedIds.Activities.PortfolioPageAssignment,
                    ResourceId = SeedIds.Resources.PortfolioOskarTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.PortfolioLinaTurnin,
                    ActivityId = SeedIds.Activities.PortfolioPageAssignment,
                    ResourceId = SeedIds.Resources.PortfolioLinaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.TodoAppMariaTurnin,
                    ActivityId = SeedIds.Activities.TodoAppAssignment,
                    ResourceId = SeedIds.Resources.TodoAppMariaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.TodoAppEmmaTurnin,
                    ActivityId = SeedIds.Activities.TodoAppAssignment,
                    ResourceId = SeedIds.Resources.TodoAppEmmaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.TodoAppOskarTurnin,
                    ActivityId = SeedIds.Activities.TodoAppAssignment,
                    ResourceId = SeedIds.Resources.TodoAppOskarTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.TodoAppViktorTurnin,
                    ActivityId = SeedIds.Activities.TodoAppAssignment,
                    ResourceId = SeedIds.Resources.TodoAppViktorTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.CrudApiJohanTurnin,
                    ActivityId = SeedIds.Activities.CrudApiAssignment,
                    ResourceId = SeedIds.Resources.CrudApiJohanTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.CrudApiErikTurnin,
                    ActivityId = SeedIds.Activities.CrudApiAssignment,
                    ResourceId = SeedIds.Resources.CrudApiErikTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.CrudApiAndersTurnin,
                    ActivityId = SeedIds.Activities.CrudApiAssignment,
                    ResourceId = SeedIds.Resources.CrudApiAndersTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.CrudApiElinTurnin,
                    ActivityId = SeedIds.Activities.CrudApiAssignment,
                    ResourceId = SeedIds.Resources.CrudApiElinTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DockerizeSaraTurnin,
                    ActivityId = SeedIds.Activities.DockerizeAppAssignment,
                    ResourceId = SeedIds.Resources.DockerizeSaraTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DockerizeFredrikTurnin,
                    ActivityId = SeedIds.Activities.DockerizeAppAssignment,
                    ResourceId = SeedIds.Resources.DockerizeFredrikTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DockerizeNinaTurnin,
                    ActivityId = SeedIds.Activities.DockerizeAppAssignment,
                    ResourceId = SeedIds.Resources.DockerizeNinaTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DockerizeJosefinTurnin,
                    ActivityId = SeedIds.Activities.DockerizeAppAssignment,
                    ResourceId = SeedIds.Resources.DockerizeJosefinTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DeployPipelineSaraTurnin,
                    ActivityId = SeedIds.Activities.DeploymentPipelineAssignment,
                    ResourceId = SeedIds.Resources.DeployPipelineSaraTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DeployPipelineFredrikTurnin,
                    ActivityId = SeedIds.Activities.DeploymentPipelineAssignment,
                    ResourceId = SeedIds.Resources.DeployPipelineFredrikTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DeployPipelineJosefinTurnin,
                    ActivityId = SeedIds.Activities.DeploymentPipelineAssignment,
                    ResourceId = SeedIds.Resources.DeployPipelineJosefinTurnin
                },
                new ActivityResource
                {
                    Id = SeedIds.ActivityResources.DeployPipelineMartinTurnin,
                    ActivityId = SeedIds.Activities.DeploymentPipelineAssignment,
                    ResourceId = SeedIds.Resources.DeployPipelineMartinTurnin
                }
            );
        }
    }
}
