using Lms_backend.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasKey(m => m.ModuleId);

            builder.Property(m => m.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(m => m.UpdatedAt)
                    .HasColumnType("datetime2");

            builder.Property(m => m.Name)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(m => m.Description)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");

            builder.Property(m => m.DurationDays)
                    .HasColumnType("int");

            builder.HasData(

                            //Seed-data
                            );
        }
    }
}
