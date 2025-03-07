using firstProject.Models;

namespace CoreProject.Middlewares;

public class LogMiddleware
{
    private RequestDelegate next;
    public LogMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext c)
    {
        await c.Response.WriteAsync($"Our Log Middleware start\n");
        await next(c);
        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} "
            + $" Success: {c.Items["success"]}"
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        await c.Response.WriteAsync("Our Log Middleware end\n");
    }
}

public static partial class MiddlewareExtantion
{
    public static void UseLog(this IApplicationBuilder a)
    {
        a.UseMiddleware<LogMiddleware>();
    }
}