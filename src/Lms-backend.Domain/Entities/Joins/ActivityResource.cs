using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
namespace Lms_backend.Domain.Entities.Joins
{
    public record ActivityResource : IResourceJoin
    {
        public Guid Id { get; set; }

        public Guid ActivityId { get; set; }

        public Guid ResourceId { get; set; }

        public Activity? Activity { get; set; }

        public Resource? Resource { get; set; }
    }
}
