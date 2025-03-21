using System.Diagnostics;
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
        // await c.Response.WriteAsync($"Our Log Middleware start\n");
        // await next(c);
        // Console.WriteLine($"{c.Request.Path}.{c.Request.Method} "
        //     + $" Success: {c.Items["success"]}"
        //     + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");
        // // Console.WriteLine("vcxbcxb");
        // await c.Response.WriteAsync("Our Log Middleware end\n");
         Console.WriteLine($"{c.Request.Path}.{c.Request.Method} start");
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        Console.WriteLine($"{c.Request.Path}.{c.Request.Method} end after {sw.ElapsedMilliseconds} ms");
    } //
}

public static partial class MiddlewareExtantion
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder a)
    {
        return a.UseMiddleware<LogMiddleware>();
    }
}