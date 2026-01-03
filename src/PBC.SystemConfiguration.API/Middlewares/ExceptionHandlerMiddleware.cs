using System.Net;
using System.Text.Json;
using PBC.SystemConfiguration.Application.Dtos.Common;
using PBC.SystemConfiguration.Domain.Exceptions;

namespace PBC.SystemConfiguration.API.Middlewares;

/// <summary>
/// Middleware responsible for handling exceptions globally across the HTTP request pipeline.
/// </summary>
/// <remarks>
/// This middleware intercepts unhandled exceptions thrown during request processing and
/// converts them into standardized JSON responses. Domain-specific exceptions
/// (<see cref="DomainException"/>) are translated into meaningful HTTP status codes,
/// while all other unhandled exceptions result in a generic 500 Internal Server Error
/// response.
///
/// Centralizing exception handling ensures consistent API error responses, keeps
/// controllers clean, and prevents leaking unhandled exceptions to API consumers.
/// </remarks>
public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Executes the middleware logic for handling exceptions during HTTP request processing.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <remarks>
    /// Wraps the remaining request pipeline in a try/catch block to intercept exceptions.
    /// Domain exceptions are handled explicitly to return meaningful error responses,
    /// while unexpected exceptions are captured and translated into a generic
    /// internal server error response.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DomainException e)
        {
            await HandleDomainException(context, e);
        }
        catch (Exception e)
        {
            await HandleUnhandledException(context, e);
        }
    }
    
    private static Task HandleDomainException(HttpContext context, DomainException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception.StatusCode;

        var response = new Response<object?>(exception.StatusCode, exception.Message, false, null);
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static Task HandleUnhandledException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new Response<object?>((int)HttpStatusCode.InternalServerError, exception.Message, false, null);
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}