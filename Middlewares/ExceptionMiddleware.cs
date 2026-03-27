using AuthDemo.Common;
using AuthDemo.Exceptions;
using AuthDemo.Models;
using AuthDemo.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace AuthDemo.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly EmailSetting _settings;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env,
            IOptions<EmailSetting> settings
            )
        {
            _next = next;
            _logger = logger;
            _env = env;
            _settings = settings.Value;
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
                if (_settings.SendErrorEmail)
                {
                    _ = SendErrorEmailAsync(context, ex);
                }
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

        private async Task SendErrorEmailAsync(HttpContext context, Exception ex)
        {
            try
            {
                var emailService = context.RequestServices.GetRequiredService<IEmailService>();
                var request = context.Request;

                var errorDetails = $@"
            <h2>🚨 Unhandled Exception</h2>
            <p><strong>Message:</strong> {ex.Message}</p>
            <p><strong>Path:</strong> {request.Path}</p>
            <p><strong>Method:</strong> {request.Method}</p>
            <p><strong>Query:</strong> {request.QueryString}</p>
            <p><strong>User:</strong> {context.User?.Identity?.Name ?? "Anonymous"}</p>
            <p><strong>Time:</strong> {DateTime.UtcNow}</p>
            <h3>Stack Trace</h3>
            <pre>{ex.StackTrace}</pre>
        ";

                var emailRequest = new EmailRequest
                {
                    To = "your-email@gmail.com", // 🔥 your email
                    Subject = "🚨 Application Error",
                    Body = errorDetails,
                    IsHtml = true,
                    Cc = new List<string>(),       // optional
                    Bcc = new List<string>(),      // optional
                    AttachmentsPaths = new List<string>() // optional
                };

                await emailService.SendEmailAsync(emailRequest);
            }
            catch (Exception emailEx)
            {
                var logger = context.RequestServices.GetRequiredService<ILogger<ExceptionMiddleware>>();
                logger.LogError(emailEx, "Failed to send error email");
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}