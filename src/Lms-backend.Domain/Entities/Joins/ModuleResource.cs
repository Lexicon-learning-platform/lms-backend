using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Interfaces;
namespace Lms_backend.Domain.Entities.Joins
{
    public record ModuleResource : IResourceJoin
    {
        public Guid Id { get; set; }

        public Guid ModuleId { get; set; }

        public Guid ResourceId { get; set; }

        public Module? Module { get; set; }

        public Resource? Resource { get; set; }
    }
}
