namespace Lms_backend.Api.Entities
{
    public record Course
    {
        public Guid CourseId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public DateTime? StartDate { get; set; }

        public int? Duration { get; set; }

        public ICollection<Module>? Modules { get; set; }
    }
}
