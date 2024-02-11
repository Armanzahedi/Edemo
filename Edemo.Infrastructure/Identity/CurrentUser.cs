using System.Security.Claims;
using Edemo.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Edemo.Infrastructure.Identity;

public class CurrentUser(IHttpContextAccessor context) : ICurrentUser
{
    private string? Id => context.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public Guid? UserId => Id != null ? Guid.Parse(Id) : null;
}