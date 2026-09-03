using Lms_backend.Domain.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations.Joins
{
    public class CourseResourceConfiguration : IEntityTypeConfiguration<CourseResource>
    {
        public void Configure(EntityTypeBuilder<CourseResource> builder)
        {
            builder.HasOne(cr => cr.Course)
        .WithMany(c => c.Resources)
        .HasForeignKey(cr => cr.CourseId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(mr => mr.Resource)
        .WithMany()
        .HasForeignKey(mr => mr.ResourceId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);
        }
    }
}
