using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace HospitalManagement.API.Middleware
{
    public class ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        // ✅ STATIC (FIX PERFORMANCE WARNING)
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public async Task Invoke(HttpContext context)
        {
            var traceId = context.TraceIdentifier;

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{traceId}] Unhandled Exception", traceId);

                if (context.Response.HasStarted)
                    throw;

                context.Response.Clear();
                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    ValidationException => HttpStatusCode.BadRequest,
                    ArgumentException => HttpStatusCode.BadRequest,
                    UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                    SecurityTokenException => HttpStatusCode.Unauthorized,
                    KeyNotFoundException => HttpStatusCode.NotFound,
                    ApplicationException => HttpStatusCode.BadRequest,
                    _ => HttpStatusCode.InternalServerError
                };

                context.Response.StatusCode = (int)statusCode;

                var response = new
                {
                    success = false,
                    message = _env.IsDevelopment()
                        ? ex.Message
                        : "An unexpected error occurred.",
                    statusCode = context.Response.StatusCode,
                    traceId,
                    errors = ex is ValidationException valEx
                        ? valEx.Errors.Select(e => new
                        {
                            field = e.PropertyName,
                            error = e.ErrorMessage
                        })
                        : null
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response, _jsonOptions) // ✅ reused
                );
            }
        }
    }
}