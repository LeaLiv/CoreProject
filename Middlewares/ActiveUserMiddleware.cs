
using System.Security.Claims;
using firstProject.Models;

namespace CoreProject.Middlewares;
public class ActiveUserMiddleware
{
    private readonly RequestDelegate _next;

    public ActiveUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ActiveUser activeUser)
    {
        var user = context.User;
        if (user.Identity?.IsAuthenticated == true)
        {
            activeUser.UserId = user.FindFirst(ClaimTypes.Name)?.Value;
            activeUser.Role = user.FindFirst(ClaimTypes.Role)?.Value;
        }

        await _next(context);
    }
}
