using Lms_backend.Api.Entities;
using Lms_backend.Api.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations.Joins
{
    public class ActivityResourceConfiguration : IEntityTypeConfiguration<ActivityResource>
    {
        public void Configure(EntityTypeBuilder<ActivityResource> builder)
        {
            builder.HasKey(j => j.Id);

        }
    }
}
