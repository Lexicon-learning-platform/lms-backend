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
            CreatedBy = UserMapper.ToSimpleDto(entity.Owner),
        };
    }

    public static IEnumerable<ResourceDto> ToStandardDto(IEnumerable<Resource> entities)
    {
        foreach (var item in entities)
        {
            yield return ToStandardDto(item);
        }
    }

    public static ResourceForChangeDto ToChangeDto(Resource entity)
    {
        return new ResourceForChangeDto()
        {
            Name = entity.Name,
            Description = entity.Description,
            Type = entity.ResourceType,
            Data = entity.Data ?? string.Empty,
        };
    }
}
