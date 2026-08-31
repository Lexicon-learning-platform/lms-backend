using Lms_backend.Api.Entities.Joins;

namespace Lms_backend.Api.Entities
{
    public record Module
    {
        public Guid ModuleId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? DurationDays { get; set; }

        public ICollection<ModuleResource> ModuleResources { get; set; } = new List<ModuleResource>();
        public ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

        public ICollection<Activity> Activities { get; set; } = new List<Activity>();

    }
}
