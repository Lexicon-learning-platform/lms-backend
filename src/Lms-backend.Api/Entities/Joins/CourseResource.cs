namespace Lms_backend.Api.Entities.Joins
{
    public record CourseResource
    {
        public Guid Id { get; set; }

        public int CourseId { get; set; }

        public int ResourceId { get; set; }
    }
}
