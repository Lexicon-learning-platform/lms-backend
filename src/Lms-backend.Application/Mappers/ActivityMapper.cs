using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Application.Mappers;

public static class ActivityMapper
{
    public static ActivityDto ToStandardDto(Activity entity)
    {
        return new ActivityDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Duration = entity.DurationMinutes
        };
    }

    public static IEnumerable<ActivityDto> ToStandardDto(IEnumerable<Activity> entities)
    {
        foreach (var item in entities)
        {
            yield return ToStandardDto(item);
        }
    }

    public static ActivityExtendedDto ToExtendedDto(Activity entity, IEnumerable<Resource> resources)
    {
        return new ActivityExtendedDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            Description = entity.Description,
            StartOffest = entity.StartTimeOffset,
            Duration = entity.DurationMinutes,
            Resources = ResourceMapper.ToSimpleDto(resources),
        };
    }
}
