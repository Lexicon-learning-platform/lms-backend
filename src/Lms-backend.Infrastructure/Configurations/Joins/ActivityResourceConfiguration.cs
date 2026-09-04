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
                }
            );
        }
    }
}
