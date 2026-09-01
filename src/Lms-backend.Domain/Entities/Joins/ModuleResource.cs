using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
namespace Lms_backend.Domain.Entities.Joins
{
    public record ModuleResource : IEntity
    {
        public Guid Id { get; set; }

        public int ModuleId { get; set; }

        public int ResourceId { get; set; }

        public Module? Module { get; set; }

        public Resource? Resource { get; set; }
    }
}
