
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try 
        {
            await next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
            ex, "Exception occurred: {Message}", ex.Message);

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erreur Serveur",
                Detail = ex.Message,
            };

            context.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await context.Response.WriteAsJsonAsync(problemDetails);

        }
    }   
}
