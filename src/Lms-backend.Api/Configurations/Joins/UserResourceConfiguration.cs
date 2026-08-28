using Lms_backend.Api.Entities;
using Lms_backend.Api.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations.Joins
{
    public class UserResourceConfiguration : IEntityTypeConfiguration<UserResource>
    {
        public void Configure(EntityTypeBuilder<UserResource> builder)
        {
            builder.HasKey(j => j.Id);
        }
    }
}
