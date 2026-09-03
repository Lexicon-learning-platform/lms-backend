using Lms_backend.Domain.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations.Joins
{
    public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
    {
        public void Configure(EntityTypeBuilder<CourseModule> builder)
        {

            builder.HasOne(cm => cm.Course)
.WithMany(c => c.Modules)
.HasForeignKey(cm => cm.CourseId)
.OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(cm => cm.Module)
        .WithMany(m => m.Courses)
        .HasForeignKey(cm => cm.ModuleId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);

            builder.HasIndex(j => new { j.CourseId, j.ModuleId })
                    .IsUnique();
        }
    }
}
