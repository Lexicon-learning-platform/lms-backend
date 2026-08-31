using Lms_backend.Api.Entities;
using Lms_backend.Api.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations.Joins
{
    public class CourseModuleConfiguration : IEntityTypeConfiguration<CourseModule>
    {
        public void Configure(EntityTypeBuilder<CourseModule> builder)
        {

            builder.HasOne(cm => cm.Course)
.WithMany(c => c.CourseModules)
.HasForeignKey(cm => cm.CourseId)
.OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(cm => cm.Module)
        .WithMany(m => m.CourseModules)
        .HasForeignKey(cm => cm.ModuleId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);

        }
    }
}
