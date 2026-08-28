using Lms_backend.Api.Enums;

namespace Lms_backend.Api.Entities
{
    public record Activity
    {
        public Guid ActivityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int? ModuleId { get; set; }

        public ActivityType ActivityType { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int StartTimeOffset { get; set; }

        public int DurationMinutes { get; set; }
    }
}
