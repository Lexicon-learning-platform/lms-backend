using Lms_backend.Domain.Entities.Joins;
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
        }
    }
}
