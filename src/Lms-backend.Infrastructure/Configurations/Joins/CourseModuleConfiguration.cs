using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Configurations;
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

            builder.HasData(
                new CourseModule
                {
                    Id = SeedIds.CourseModules.FullStackGit,
                    CourseId = SeedIds.Courses.FullStack,
                    ModuleId = SeedIds.Modules.Git,
                    StartTimeOffset = 0
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.FullStackFrontend,
                    CourseId = SeedIds.Courses.FullStack,
                    ModuleId = SeedIds.Modules.Frontend,
                    StartTimeOffset = 7
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.FullStackReact,
                    CourseId = SeedIds.Courses.FullStack,
                    ModuleId = SeedIds.Modules.React,
                    StartTimeOffset = 28
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.BackendGit,
                    CourseId = SeedIds.Courses.Backend,
                    ModuleId = SeedIds.Modules.Git,
                    StartTimeOffset = 0
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.BackendCSharp,
                    CourseId = SeedIds.Courses.Backend,
                    ModuleId = SeedIds.Modules.CSharp,
                    StartTimeOffset = 7
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.BackendAspNetCore,
                    CourseId = SeedIds.Courses.Backend,
                    ModuleId = SeedIds.Modules.AspNetCore,
                    StartTimeOffset = 28
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.CloudDevOpsDocker,
                    CourseId = SeedIds.Courses.CloudDevOps,
                    ModuleId = SeedIds.Modules.Docker,
                    StartTimeOffset = 0
                },
                new CourseModule
                {
                    Id = SeedIds.CourseModules.CloudDevOpsCiCd,
                    CourseId = SeedIds.Courses.CloudDevOps,
                    ModuleId = SeedIds.Modules.CiCd,
                    StartTimeOffset = 14
                }
            );
        }
    }
}
