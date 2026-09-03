using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities.Joins
{
    public record ActivityResource : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ResourceId { get; set; }

        [ForeignKey("ActivityId")]
        public Activity Activity { get; set; } = default!;

        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; } = default!;
    }
}
