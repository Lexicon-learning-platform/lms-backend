using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Application.Mappers;

public static class ActivityMapper
{
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