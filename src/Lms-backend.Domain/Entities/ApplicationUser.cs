using Lms_backend.Domain.Entities.Joins;
using Lms_backend.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Lms_backend.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>, IEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string GivenName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public Guid? CourseId { get; set; }

        public Course? Course { get; set; } = null;

        public ICollection<UserResource> Resources { get; set; } = [];

    }
}
