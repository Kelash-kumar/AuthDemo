using AuthDemo.Common;
using AuthDemo.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;

namespace AuthDemo.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                // Only intercept if response hasn't started yet
                if (!context.Response.HasStarted)
                {
                    if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                    {
                        await WriteJsonAsync(context, ApiResponse<object>.Unauthorized(
                            "You must be logged in to access this resource."));
                    }
                    else if (context.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                    {
                        await WriteJsonAsync(context, ApiResponse<object>.Forbidden(
                            "You do not have permission to access this resource."));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // If response already started, we can't do anything
            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Response already started, cannot modify headers.");
                return;
            }

            ApiResponse<object> response = exception switch
            {
                NotFoundException ex => ApiResponse<object>.NotFound(ex.Message),
                BadRequestException ex => ApiResponse<object>.Fail(ex.Message, 400),
                Exceptions.ValidationException ex => ApiResponse<object>.ValidationError(ex.Errors),
                UnauthorizedException ex => ApiResponse<object>.Unauthorized(ex.Message),
                ForbiddenException ex => ApiResponse<object>.Forbidden(ex.Message),
                ConflictException ex => ApiResponse<object>.Fail(ex.Message, 409),
                UnauthorizedAccessException => ApiResponse<object>.Unauthorized(),
                _ => ApiResponse<object>.ServerError(
                    _env.IsDevelopment() ? exception.Message : "An unexpected error occurred."),
            };

            await WriteJsonAsync(context, response);
        }

        private static async Task WriteJsonAsync(HttpContext context, ApiResponse<object> response)
        {
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response, options);
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}