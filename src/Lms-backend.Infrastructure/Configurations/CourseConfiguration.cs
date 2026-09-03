using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CreatedAt)
                    .IsRequired();

            builder.Property(c => c.Name)
                    .HasMaxLength(50);

            builder.Property(c => c.Description)
                    .HasMaxLength(200);

            builder.HasData(

                //Seed-data
                );
        }
    }
}
