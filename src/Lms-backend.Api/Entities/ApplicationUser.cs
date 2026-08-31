using Lms_backend.Api.Entities.Joins;

namespace Lms_backend.Api.Entities
{
    public record ApplicationUser
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
