namespace HospitalManagement.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var traceId = context.TraceIdentifier;
            context.Response.Headers["X-Trace-Id"] = traceId;

            var startTime = DateTime.UtcNow;

            var user = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            try
            {
                _logger.LogInformation(
                    "[{traceId}] Request: {method} {path} | User: {user}",
                    traceId,
                    context.Request.Method,
                    context.Request.Path,
                    user
                );

                await _next(context);

                var duration = DateTime.UtcNow - startTime;

                _logger.LogInformation(
                    "[{traceId}] Response: {statusCode} in {duration} ms",
                    traceId,
                    context.Response.StatusCode,
                    duration.TotalMilliseconds
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{traceId}] ERROR", traceId);
                throw;
            }
        }
    }
}
