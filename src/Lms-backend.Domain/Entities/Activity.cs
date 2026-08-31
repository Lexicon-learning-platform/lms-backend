using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Domain.Entities
{
    public record Activity
    {
        public Guid ActivityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? ModuleId { get; set; }

        public Module? Module { get; set; } = null;

        public ActivityType ActivityType { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int StartTimeOffset { get; set; }

        public int DurationMinutes { get; set; }

        public ICollection<ActivityResource> ActivityResources { get; set; } = new List<ActivityResource>();
    }
}
