using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Lms_backend.Domain.Interfaces;

namespace Lms_backend.Domain.Entities.Joins
{
    public record ModuleResource : IEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid ModuleId { get; set; }

        public Guid ResourceId { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; } = default!;

        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; } = default!;
    }
}
