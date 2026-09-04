using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations.Joins
{
    public class UserResourceConfiguration : IEntityTypeConfiguration<UserResource>
    {
        public void Configure(EntityTypeBuilder<UserResource> builder)
        {
            builder.HasOne(er => er.User)
        .WithMany(u => u.Resources)
        .HasForeignKey(er => er.UserId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(mr => mr.Resource)
        .WithMany()
        .HasForeignKey(mr => mr.ResourceId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);

            builder.HasIndex(j => new { j.UserId, j.ResourceId })
                    .IsUnique();

            builder.HasData(
                new UserResource
                {
                    Id = SeedIds.UserResources.MariaGitNotes,
                    UserId = SeedIds.Users.Maria,
                    ResourceId = SeedIds.Resources.MariaGitNotes
                }
            );
        }
    }
}
