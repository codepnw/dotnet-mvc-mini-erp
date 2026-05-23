using MiniERP.Mvc.DTOs.Requests;
using MiniERP.Mvc.Entities;

namespace MiniERP.Mvc.Mappings;

public static class UserMappings
{
    public static User ToUserEntity(this UserCreateRequest request, string hashedPassword)
    {
        return new User
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            FirstName = request.FirstName,
            LastName = request.LastName,
        };
    }
}