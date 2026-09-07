using Lms_backend.Application.Models;
using Lms_backend.Domain.Entities;

namespace Lms_backend.Application.Mappers;

public static class UserMapper
{
    public static UserSimpleDto ToSimpleDto(ApplicationUser entity)
    {
        return new UserSimpleDto()
        {
            Id = entity.Id,
            GivenName = entity.GivenName,
            LastName = entity.LastName,
        };
    }
}
