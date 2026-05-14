using MiniERP.Mvc.DTOs;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Mappings;

public static class UserMappings
{
    public static User ToUserEntity(this UserCreateDto dto, string hashedPassword)
    {
        return new User
        {
            Email = dto.Email,
            PasswordHash = hashedPassword,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        };
    }
}