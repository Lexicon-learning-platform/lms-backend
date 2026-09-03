using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
using Microsoft.AspNetCore.Identity;

namespace Lms_backend.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? GivenName { get; set; }

        public string? LastName { get; set; }

        public int? CourseId { get; set; }

        public Course? Course { get; set; } = null;

        public ICollection<UserResource> UserResources { get; set; } = new List<UserResource>();

    }
}
