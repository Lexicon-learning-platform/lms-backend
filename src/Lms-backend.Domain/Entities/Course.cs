using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities
{
    public record Course: IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public int? Duration { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

        public ICollection<Module>? Modules { get; set; } = new List<Module>();

        public ICollection<CourseResource> CourseResources { get; set; } = new List<CourseResource>();

        public ICollection<CourseModule> CourseModules { get; set; } = new List<CourseModule>();

    }
}
