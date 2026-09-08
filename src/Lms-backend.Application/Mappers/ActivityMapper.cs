using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Application.Mappers;

public static class ActivityMapper
{
    public static ActivityDto ToDto(Activity entity)
    {
        return new ActivityDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Duration = entity.DurationMinutes
        };
    }
}