using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.API.Exceptions;

namespace ProjectManagement.API.Handlers;

public sealed class GlobalsExceptionHandler(
    ILogger<GlobalsExceptionHandler> logger) 
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);
        
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };
        
        var (status, title) = exception switch
        {
            NotFoundException => (
                StatusCodes.Status404NotFound,
                "Resource not found"),
            
            ConflictException => (
                StatusCodes.Status409Conflict,
                "Resource already exists"),
            
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error")
        };
        
        problemDetails.Status = status;
        problemDetails.Title = title;
        problemDetails.Detail = exception.Message;

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}