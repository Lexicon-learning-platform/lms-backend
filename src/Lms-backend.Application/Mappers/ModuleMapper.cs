using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;
using Lms_backend.Domain.Entities.Joins;
namespace Lms_backend.Application.Mappers;

public static class ModuleMapper
{
    public static ModuleDto ToStandardDto(Module entity)
    {
        return new ModuleDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Duration = entity.Duration,
            Activities = entity.Activities.Select(ActivityMapper.ToSimpleDto),
        };
    }

    public static IEnumerable<ModuleDto> ToStandardDto(IEnumerable<Module> entities)
    {
        foreach (var item in entities)
        {
            yield return ToStandardDto(item);
        }
    }

    public static ModuleExtendedDto ToExtendedDto(Module entity, CourseModule? join)
    {
        return new ModuleExtendedDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            Description = entity.Description,
            StartOffset = join?.StartTimeOffset,
            Duration = entity.Duration,
            Activities = entity.Activities.Select(a => ActivityMapper.ToStandardDto(a)),
            Resources = entity.Resources.Select(r => ResourceMapper.ToStandardDto(r.Resource)),
        };
    }

    public static ModuleSimpleDto ToSimpleDto(Module entity)
    {
        return new ModuleSimpleDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.Duration,
        };
    }

    public static ModuleForChangeDto ToChangeDto(Module entity)
    {
        return new ModuleForChangeDto()
        {
            Name = entity.Name,
            Description = entity.Description,
            Duration = entity.Duration,
            ActivityIds = [.. entity.Activities.Select(a => a.Id)],
        };
    }

    public static Module ToEntity(ModuleForChangeDto dto)
    {
        return new Module()
        {
            Name = dto.Name,
            Description = dto.Description,
            Duration = dto.Duration,
        };
    }
}
