using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Application.Mappers;

public static class ResourceMapper
{
    public static ResourceDto ToStandardDto(Resource entity)
    {
        return new ResourceDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Type = entity.ResourceType,
            Data = entity.Data ?? string.Empty,
            CreatedBy = new UserSimpleDto()
            {
                Id = entity.OwnerId,
                GivenName = entity.Owner.GivenName,
                LastName = entity.Owner.LastName,
            },
        };
    }
}
