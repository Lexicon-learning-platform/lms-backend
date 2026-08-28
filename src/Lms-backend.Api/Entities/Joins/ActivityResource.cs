namespace Lms_backend.Api.Entities.Joins
{
    public record ActivityResource
    {
        public Guid Id { get; set; }

        public int ActivityId { get; set; }

        public int ResourceId { get; set; }

        public Activity? Activity { get; set; }

        public Resource? Resource { get; set; }
    }
}
