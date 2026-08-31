using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
namespace Lms_backend.Domain.Entities.Joins
{
    public record ActivityResource : IEntity
    {
        public Guid Id { get; set; }

        public int ActivityId { get; set; }

        public int ResourceId { get; set; }

        public Activity? Activity { get; set; }

        public Resource? Resource { get; set; }
    }
}
