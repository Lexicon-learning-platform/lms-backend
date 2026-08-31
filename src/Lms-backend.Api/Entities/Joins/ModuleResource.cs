namespace Lms_backend.Api.Entities.Joins
{
    public record ModuleResource
    {
        public Guid Id { get; set; }

        public int ModuleId { get; set; }

        public int ResourceId { get; set; }

        public Module? Module { get; set; }

        public Resource? Resource { get; set; }
    }
}
