using System.ComponentModel.DataAnnotations;
using Lms_backend.Domain.Enums;

namespace Lms_backend.Domain.Entities
{
    public record Resource
    {
        [Key]
        public Guid ResourceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid OwnerId { get; set; }

        public ApplicationUser Owner { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public ResourceType ResourceType { get; set; }

        public string? Data { get; set; }
    }
}
