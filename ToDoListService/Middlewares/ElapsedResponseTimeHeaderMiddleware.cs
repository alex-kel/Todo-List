using System.Diagnostics;

namespace ToDoListService.Middlewares;

public class ElapsedResponseTimeHeaderMiddleware(RequestDelegate next)
{
    private const string ElapsedTimeHeader = "X-ElapsedTime";
    
    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        await next(context);

        context.Response.Headers.Append(ElapsedTimeHeader, stopWatch.ElapsedMilliseconds.ToString());
    }
}

public static class ElapsedResponseTimeHeaderMiddlewareExtensions
{
    public static IApplicationBuilder UseElapsedResponseTimeHeader(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ElapsedResponseTimeHeaderMiddleware>();
    }
}