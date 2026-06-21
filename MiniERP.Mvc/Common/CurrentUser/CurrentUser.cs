using System.Security.Claims;
using MiniERP.Mvc.Common.Constants;

namespace MiniERP.Mvc.Common.CurrentUser;

public interface ICurrentUser
{
    string? UserId { get; }
    bool IsInRole(string role);
    bool IsAdmin { get; }
}

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;

    public bool IsAdmin => IsInRole(Roles.Admin);
}