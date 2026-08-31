namespace Lms_backend.Api.Entities.Joins
{
    public record CourseResource
    {
        public Guid Id { get; set; }

        public int CourseId { get; set; }

        public int ResourceId { get; set; }

        public Course? Course { get; set; }

        public Resource? Resource { get; set; }
    }
}
