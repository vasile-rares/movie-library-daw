namespace MovieLibrary.API.Middlewares
{
  public class ExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
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
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An unhandled exception occurred");
        await HandleExceptionAsync(context, ex, _env);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
    {
      context.Response.ContentType = "application/json";

      var response = new
      {
        message = exception.Message,
        statusCode = exception switch
        {
          KeyNotFoundException => StatusCodes.Status404NotFound,
          ArgumentException => StatusCodes.Status400BadRequest,
          UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
          _ => StatusCodes.Status500InternalServerError
        },
        details = env.IsDevelopment() ? exception.StackTrace : null
      };

      context.Response.StatusCode = response.statusCode;
      return context.Response.WriteAsJsonAsync(response);
    }
  }
}
