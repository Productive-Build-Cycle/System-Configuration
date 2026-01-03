using Serilog.Context;

namespace PBC.SystemConfiguration.API.Middlewares;

public class RequestBodyLoggingMiddleware(RequestDelegate next, ILogger<RequestBodyLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Only log POST, PUT, PATCH requests and if the content type is relevant (e.g., JSON)
        if (context.Request.Method == HttpMethods.Post ||
            context.Request.Method == HttpMethods.Put ||
            context.Request.Method == HttpMethods.Patch)
        {
            // Ensure the request body can be read multiple times
            context.Request.EnableBuffering();

            // Leave the body open so the next middleware can read it.
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            // Reset the position of the stream to the beginning
            context.Request.Body.Position = 0;

            using (LogContext.PushProperty("RequestBody", body, true)) // true for destructuring (JSON)
            {
                await next(context);
            }
        }
        else
        {
            await next(context);
        }
    }
}

public static class RequestBodyLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestBodyLogging(this IApplicationBuilder builder) =>
        builder.UseMiddleware<RequestBodyLoggingMiddleware>();
}