using Lms_backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lms_backend.Infrastructure.Configurations
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasOne<Course>(u => u.Course)
                    .WithMany(c => c.Users)
                    .HasForeignKey(u => u.CourseId);

            builder.Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("now()");

            builder.Property(u => u.UpdatedAt)
                    .HasDefaultValueSql("now()");

            builder.Property(u => u.GivenName)
                    .HasMaxLength(50);

            builder.Property(u => u.LastName)
                    .HasMaxLength(50);

            builder.HasData(
                new ApplicationUser
                {
                    Id = SeedIds.Users.Alex,
                    ConcurrencyStamp = "268d2cf5-1946-4e8e-b915-7f66ea6abed8",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Alex",
                    LastName = "Nilsson",
                    CourseId = null
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Maria,
                    ConcurrencyStamp = "01f429a1-8204-4597-bc2a-e763ee8e1e9b",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Maria",
                    LastName = "Svensson",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Johan,
                    ConcurrencyStamp = "48fd668d-3a3b-40d5-9c9e-e96de93a4458",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Johan",
                    LastName = "Berg",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Sara,
                    ConcurrencyStamp = "4a08b3ba-e848-4ca6-aee6-51fa69ded960",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Sara",
                    LastName = "Lindqvist",
                    CourseId = SeedIds.Courses.CloudDevOps
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Emma,
                    ConcurrencyStamp = "8b87ae00-f38f-4dfd-9470-fae499b9d999",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Emma",
                    LastName = "Karlsson",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Oskar,
                    ConcurrencyStamp = "d5ed9a83-ce9a-444a-9e5e-48dbc96d605f",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Oskar",
                    LastName = "Lindberg",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Lina,
                    ConcurrencyStamp = "76347267-0422-4483-a70e-c72338e79a71",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Lina",
                    LastName = "Hakansson",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Viktor,
                    ConcurrencyStamp = "f48ed268-1ee5-4d2f-9683-04b6f5c1bf79",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Viktor",
                    LastName = "Astrom",
                    CourseId = SeedIds.Courses.FullStack
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Erik,
                    ConcurrencyStamp = "626c18fe-9584-4585-a6e9-394aba0ba1b9",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Erik",
                    LastName = "Holm",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Sofia,
                    ConcurrencyStamp = "c2ea1c6d-87ff-4890-85d7-db834a55cf1e",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Sofia",
                    LastName = "Bergstrom",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Anders,
                    ConcurrencyStamp = "09fa74cb-b7c6-4c36-8f6c-b23094594822",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Anders",
                    LastName = "Nystrom",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Elin,
                    ConcurrencyStamp = "8b5e418d-9348-4f50-aaa1-7a343c1cdaea",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Elin",
                    LastName = "Forsberg",
                    CourseId = SeedIds.Courses.Backend
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Fredrik,
                    ConcurrencyStamp = "ea445ccf-fbef-43b4-acc1-ffe255d95097",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Fredrik",
                    LastName = "Dahl",
                    CourseId = SeedIds.Courses.CloudDevOps
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Nina,
                    ConcurrencyStamp = "d004077b-65a6-4a0e-87aa-d37d8df1570c",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Nina",
                    LastName = "Ekstrom",
                    CourseId = SeedIds.Courses.CloudDevOps
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Josefin,
                    ConcurrencyStamp = "ffb50c6e-e744-49af-9328-511be1055c17",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Josefin",
                    LastName = "Lund",
                    CourseId = SeedIds.Courses.CloudDevOps
                },
                new ApplicationUser
                {
                    Id = SeedIds.Users.Martin,
                    ConcurrencyStamp = "01d6c3ba-34cb-426e-a711-c8a6b6ce29fc",
                    CreatedAt = SeedIds.CreatedAt,
                    UpdatedAt = SeedIds.CreatedAt,
                    GivenName = "Martin",
                    LastName = "Oberg",
                    CourseId = SeedIds.Courses.CloudDevOps
                }
            );
        }
    }
}
