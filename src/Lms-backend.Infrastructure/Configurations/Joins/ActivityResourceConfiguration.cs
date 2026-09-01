using Lms_backend.Domain.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations.Joins
{
    public class ActivityResourceConfiguration : IEntityTypeConfiguration<ActivityResource>
    {
        public void Configure(EntityTypeBuilder<ActivityResource> builder)
        {
            builder.HasOne(ar => ar.Activity)
        .WithMany(a => a.ActivityResources)
        .HasForeignKey(ar => ar.ActivityId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(mr => mr.Resource)
        .WithMany()
        .HasForeignKey(mr => mr.ResourceId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);
        }
    }
}
