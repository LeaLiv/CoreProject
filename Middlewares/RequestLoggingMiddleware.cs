using System.Diagnostics;
using firstProject.Services;
using Serilog;

namespace CoreProject.Middlewares;
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        await _next(context);
        stopwatch.Stop();
        string userName;
    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];
    // System.Console.WriteLine(token);
        if (token== null)        
            userName="Anonymous";
        else{
            userName = UserTokenService.GetUserFromToken(token)?.userName;
            if (userName == null) userName = "Anonymous";
        }
        var logInfo = new
        {
            Timestamp = DateTime.UtcNow,
            Controller = context.Request.RouteValues["controller"],
            Action = context.Request.RouteValues["action"],
            User = userName,
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds
        };

        Log.Information("Request Info: {@logInfo}", logInfo);
    }
}
