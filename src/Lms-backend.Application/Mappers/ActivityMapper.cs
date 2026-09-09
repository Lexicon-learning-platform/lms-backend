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
            StartOffset = entity.StartTimeOffset,
            Duration = entity.DurationMinutes,
            Type = entity.ActivityType,
        };
    }

    public static IEnumerable<ActivityDto> ToStandardDto(IEnumerable<Activity> entities)
    {
        foreach (var item in entities)
        {
            yield return ToStandardDto(item);
        }
    }

    public static ActivityExtendedDto ToExtendedDto(Activity entity)
    {
        return new ActivityExtendedDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            Description = entity.Description,
            StartOffset = entity.StartTimeOffset,
            Duration = entity.DurationMinutes,
            Type = entity.ActivityType,
            Resources = entity.Resources.Select(r=> ResourceMapper.ToSimpleDto(r.Resource)),
        };
    }

    public static Activity ToEntity(ActivityForChangeDto dto, Guid moduleId)
    {
        return new Activity()
        {
            Name = dto.Name,
            Description = dto.Description,
            StartTimeOffset = dto.StartOffset,
            DurationMinutes = dto.Duration,
            ActivityType = dto.Type,
            ModuleId = moduleId,
        };
    }

    public static ActivityForChangeDto ToChangeDto(Activity entity)
    {
        return new ActivityForChangeDto()
        {
            Name = entity.Name,
            Description = entity.Description,
            StartOffset = entity.StartTimeOffset,
            Duration = entity.DurationMinutes,
            Type = entity.ActivityType,
        };
    }

    public static ActivitySimpleDto ToDto(Activity entity)
    {
        return new ActivitySimpleDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.DurationMinutes
        };
    }
}