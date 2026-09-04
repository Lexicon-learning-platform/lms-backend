using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations.Joins
{
    public class ModuleResourceConfiguration : IEntityTypeConfiguration<ModuleResource>
    {
        public void Configure(EntityTypeBuilder<ModuleResource> builder)
        {

            builder.HasOne(mr => mr.Module)
        .WithMany(m => m.Resources)
        .HasForeignKey(mr => mr.ModuleId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(mr => mr.Resource)
        .WithMany()
        .HasForeignKey(mr => mr.ResourceId)
        .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasKey(j => j.Id);

            builder.HasIndex(j => new { j.ModuleId, j.ResourceId })
                    .IsUnique();

            builder.HasData(
                new ModuleResource
                {
                    Id = SeedIds.ModuleResources.GitModuleProGitBook,
                    ModuleId = SeedIds.Modules.Git,
                    ResourceId = SeedIds.Resources.ProGitBook
                },
                new ModuleResource
                {
                    Id = SeedIds.ModuleResources.FrontendModuleMdn,
                    ModuleId = SeedIds.Modules.Frontend,
                    ResourceId = SeedIds.Resources.MdnJavaScript
                },
                new ModuleResource
                {
                    Id = SeedIds.ModuleResources.CSharpModuleConventions,
                    ModuleId = SeedIds.Modules.CSharp,
                    ResourceId = SeedIds.Resources.CSharpConventions
                },
                new ModuleResource
                {
                    Id = SeedIds.ModuleResources.AspNetCoreModuleMsLearn,
                    ModuleId = SeedIds.Modules.AspNetCore,
                    ResourceId = SeedIds.Resources.MsLearnAspNetCore
                },
                new ModuleResource
                {
                    Id = SeedIds.ModuleResources.DockerModuleDocs,
                    ModuleId = SeedIds.Modules.Docker,
                    ResourceId = SeedIds.Resources.DockerDocs
                }
            );
        }
    }
}
