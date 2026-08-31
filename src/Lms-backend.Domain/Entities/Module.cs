using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities
{
    public record Module : IEntity
    {
        public Guid Id { get; set; }

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
