using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
namespace Lms_backend.Domain.Entities.Joins
{
    public record CourseResource : IEntity
    {
        public Guid Id { get; set; }

        public Guid CourseId { get; set; }

        public Guid ResourceId { get; set; }

        public Course? Course { get; set; }

        public Resource? Resource { get; set; }
    }
}
