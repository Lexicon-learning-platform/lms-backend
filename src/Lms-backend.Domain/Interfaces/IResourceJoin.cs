using Lms_backend.Domain.Entities;

namespace Lms_backend.Domain.Interfaces;

public interface IResourceJoin : IEntity
{
    Guid ResourceId { get; set; }

    Resource? Resource { get; set; }
}
