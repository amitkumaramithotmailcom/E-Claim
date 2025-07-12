using EClaim.Infrastructure;
using System.Text.Json;

namespace E_Claim_Service
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, dbContext);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, AppDbContext dbContext)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unhandled exception on {context.Request.Path}");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var result = JsonSerializer.Serialize(new
                {
                    StatusCode = 500,
                    Message = "An unexpected error occurred. Please try again later."
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
