using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Application.Mappers;

public static class CourseMapper
{
    public static CourseWithActivitiesDto ToWithActivitiesDto(Course entity)
    {
        return new CourseWithActivitiesDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Duration = entity.Duration,
            Modules = entity.Modules
                .Select(m =>
                ModuleMapper.ToStandardDto(m.Module)),
        };
    }

    public static CourseDto ToStandardDto(Course entity)
    {
        return new CourseDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Duration = entity.Duration,
            Modules = entity.Modules.Select(m => ModuleMapper.ToSimpleDto(m.Module)),
        };
    }

    public static IEnumerable<CourseDto> ToStandardDto(IEnumerable<Course> entities)
    {
        foreach (var item in entities)
        {
            yield return ToStandardDto(item);
        }
    }

    public static CourseSimpleDto ToSimpleDto(Course entity)
    {
        return new CourseSimpleDto()
        {
            Id = entity.Id,
            Name = entity.Name,
            Duration = entity.Duration,
        };
    }

    public static CourseExtendedDto ToExtendedDto(Course entity)
    {
        return new CourseExtendedDto()
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Duration = entity.Duration,
            Modules = entity.Modules.Select(m => ModuleMapper.ToStandardDto(m.Module)),
            Resources = entity.Resources.Select(r => ResourceMapper.ToStandardDto(r.Resource)),
        };
    }

    public static CourseForChangeDto ToChangeDto(Course entity)
    {
        return new CourseForChangeDto()
        {
            Name = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Duration = entity.Duration,
            ModuleIds = [.. entity.Modules.Select(m => m.ModuleId)]
        };
    }

    public static Course ToEntity(CourseForChangeDto dto)
    {
        return new Course()
        {
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            Duration = dto.Duration,
        };
    }
}
