using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
namespace Lms_backend.Domain.Entities.Joins
{
    public record CourseResource : IEntity
    {
        public Guid Id { get; set; }

        public int CourseId { get; set; }

        public int ResourceId { get; set; }

        public Course? Course { get; set; }

        public Resource? Resource { get; set; }
    }
}
