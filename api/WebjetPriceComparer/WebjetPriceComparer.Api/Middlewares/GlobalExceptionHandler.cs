using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace WebjetPriceComparer.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var path = httpContext.Request.Path;
            var method = httpContext.Request.Method;
            var query = httpContext.Request.QueryString.ToString();

            _logger.LogError(exception,
                "Unhandled exception at {Method} {Path}{Query}", method, path, query);

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var response = new
            {
                status = httpContext.Response.StatusCode,
                title = "Internal Server Error",
                detail = exception.Message
            };

            var json = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(json, cancellationToken);

            return true;
        }
    }
}
