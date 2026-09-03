using System.ComponentModel.DataAnnotations;
using Lms_backend.Domain.Entities.Joins;

namespace Lms_backend.Domain.Entities
{
    public record ApplicationUser
    {
        [Key]
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string GivenName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public Guid? CourseId { get; set; }

        public Course? Course { get; set; } = null;

        public ICollection<UserResource> Resources { get; set; } = [];

    }
}
