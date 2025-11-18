using System.Text;
using System.Text.RegularExpressions;

namespace MovieLibrary.API.Middlewares;

public class XssProtectionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<XssProtectionMiddleware> _logger;

  // Patterns that might indicate XSS attempts
  private static readonly Regex[] XssPatterns = new[]
  {
    new Regex(@"<script[^>]*>.*?</script>", RegexOptions.IgnoreCase | RegexOptions.Compiled),
    new Regex(@"javascript:", RegexOptions.IgnoreCase | RegexOptions.Compiled),
    new Regex(@"on\w+\s*=", RegexOptions.IgnoreCase | RegexOptions.Compiled), // onclick, onerror, etc.
    new Regex(@"<iframe[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled),
    new Regex(@"<object[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled),
    new Regex(@"<embed[^>]*>", RegexOptions.IgnoreCase | RegexOptions.Compiled)
  };

  public XssProtectionMiddleware(RequestDelegate next, ILogger<XssProtectionMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    // Add security headers
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";

    // Check request body for potential XSS
    // Skip for multipart/form-data (file uploads) as reading the body as string is not appropriate
    var contentType = context.Request.ContentType;
    if ((context.Request.Method == "POST" || context.Request.Method == "PUT") &&
        (string.IsNullOrEmpty(contentType) || !contentType.Contains("multipart/form-data")))
    {
      context.Request.EnableBuffering();

      using var reader = new StreamReader(
        context.Request.Body,
        encoding: Encoding.UTF8,
        detectEncodingFromByteOrderMarks: false,
        bufferSize: 1024,
        leaveOpen: true);

      var body = await reader.ReadToEndAsync();
      context.Request.Body.Position = 0;

      // Check for XSS patterns
      foreach (var pattern in XssPatterns)
      {
        if (pattern.IsMatch(body))
        {
          _logger.LogWarning("Potential XSS attack detected in request body from {IP}",
            context.Connection.RemoteIpAddress);

          context.Response.StatusCode = StatusCodes.Status400BadRequest;
          await context.Response.WriteAsJsonAsync(new
          {
            error = "Invalid input detected. Potential security threat."
          });
          return;
        }
      }
    }

    // Check query parameters for XSS
    foreach (var param in context.Request.Query)
    {
      foreach (var pattern in XssPatterns)
      {
        if (pattern.IsMatch(param.Value.ToString()))
        {
          _logger.LogWarning("Potential XSS attack detected in query parameter '{ParamName}' from {IP}",
            param.Key, context.Connection.RemoteIpAddress);

          context.Response.StatusCode = StatusCodes.Status400BadRequest;
          await context.Response.WriteAsJsonAsync(new
          {
            error = "Invalid query parameter detected."
          });
          return;
        }
      }
    }

    await _next(context);
  }
}
