using Lms_backend.Domain.Enums;

namespace Lms_backend.Domain.Entities
{
    public record Resource
    {
        public Guid ResourceId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? OwnerId { get; set; }   // Foreign key to the entity who owns the resource

        public string? Name { get; set; }

        public string? Description { get; set; }

        public ResourceType ResourceType { get; set; }

        public string? Data { get; set; }

    }
}
