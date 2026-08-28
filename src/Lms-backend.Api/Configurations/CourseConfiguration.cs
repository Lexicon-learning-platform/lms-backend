using Lms_backend.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Api.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.CourseId);

            builder.Property(c => c.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime2");

            builder.Property(c => c.UpdatedAt)
                    .HasColumnType("datetime2");

            builder.Property(c => c.Name)
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar");

            builder.Property(c => c.Description)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");

            builder.Property(c => c.StartDate)
                    .HasColumnType("datetime2");

            builder.Property(c => c.Duration)
                    .HasColumnType("int");

            builder.HasData(

                //Seed-data
                );
        }
    }
}
