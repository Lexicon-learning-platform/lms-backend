using System.ComponentModel.DataAnnotations;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities
{
    public record Module : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public int Duration { get; set; }

        public ICollection<ModuleResource> Resources { get; set; } = [];
        public ICollection<CourseModule> Courses { get; set; } = [];

        public ICollection<Activity> Activities { get; set; } = [];

    }
}
