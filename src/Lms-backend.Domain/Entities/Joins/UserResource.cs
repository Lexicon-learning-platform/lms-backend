
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lms_backend.Domain.Entities.Joins
{
    public record UserResource
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ResourceId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = default!;

        [ForeignKey("ResourceId")]
        public Resource Resource { get; set; } = default!;
    }
}
