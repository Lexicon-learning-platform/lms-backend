using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities
{
    public record Course: IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public DateOnly StartDate { get; set; }

        public int Duration { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = [];

        public ICollection<CourseResource> Resources { get; set; } = [];

        public ICollection<CourseModule> Modules { get; set; } = [];

    }
}
