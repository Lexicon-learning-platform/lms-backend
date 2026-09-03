using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Enums;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities
{
    public record Activity : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid ModuleId { get; set; }
        [ForeignKey("ModuleId")]
        public Module Modules { get; set; } = default!;

        public ActivityType ActivityType { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int StartTimeOffset { get; set; }

        public int DurationMinutes { get; set; }

        public ICollection<ActivityResource> Resources { get; set; } = [];
    }
}
