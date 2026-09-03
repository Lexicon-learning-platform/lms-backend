using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.CreatedAt)
                    .IsRequired();

            builder.Property(m => m.Name)
                    .HasMaxLength(50);

            builder.Property(m => m.Description)
                    .HasMaxLength(200);

            builder.HasData(

                            //Seed-data
                            );
        }
    }
}
