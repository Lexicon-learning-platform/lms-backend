using System.Reflection;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Microsoft.EntityFrameworkCore;
using Module = Lms_backend.Domain.Entities.Module;

namespace Lms_backend.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Resource> Resources { get; set; }

    public DbSet<CourseModule> CourseModules { get; set; }
    public DbSet<CourseResource> CourseResources { get; set; }
    public DbSet<ModuleResource> ModuleResources { get; set; }
    public DbSet<ActivityResource> ActivityResources { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
