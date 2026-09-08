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

    public static ResourceSimpleDto ToSimpleDto(Resource entity)
    {
        return new ResourceSimpleDto()
        {
            Id = entity.Id,
            CreatedBy = UserMapper.ToSimpleDto(entity.Owner),
            Name = entity.Name,
            Type = entity.ResourceType,
        };
    }

    public static IEnumerable<ResourceSimpleDto> ToSimpleDto(IEnumerable<Resource> entities)
    {
        foreach (var item in entities)
        {
            yield return ToSimpleDto(item);
        }
    }

    public static Resource ToEntity(ResourceForChangeDto data, Guid creatorId)
    {
        return new Resource()
        {
            Name = data.Name,
            Description = data.Description,
            ResourceType = data.Type,
            Data = data.Data,
            OwnerId = creatorId,
        };
    }
}
