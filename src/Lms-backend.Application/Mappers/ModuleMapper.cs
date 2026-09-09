using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;
namespace Lms_backend.Application.Mappers;

public static class ModuleMapper
{
    public static ModuleDto ToStandardDto(Module entity)
    {
        return new ModuleDto
        {
            Id = entity.Id,
            Title = entity.Name,
            Description = entity.Description,
            Duration = entity.Duration,
            Activities = entity.Activities
                .Select(ActivityMapper.ToSimpleDto)
                .ToArray()
        };
    }
}
