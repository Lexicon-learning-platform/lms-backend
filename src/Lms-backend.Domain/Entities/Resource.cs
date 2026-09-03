using Lms_backend.Domain.Enums;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities
{
    public record Resource: IEntity
    {
        public Guid Id { get; set; }

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
