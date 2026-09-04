namespace Lms_backend.Domain.Entities.Joins
{
    public record CourseModule
    {
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid ModuleId { get; set; }

        public Course Course { get; set; } = default!;

        public Module Module { get; set; } = default!;

        public int StartTimeOffset { get; set; }
    }
}
