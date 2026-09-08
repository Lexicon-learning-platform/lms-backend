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
            Title = entity.Name,
            Description = entity.Description,
            StartDate = entity.StartDate,
            Duration = entity.Duration,
            Modules = entity.Modules
                .Select(m =>
                ModuleMapper.ToStandardDto(m.Module)).ToArray()
            
        };
    }
}