using FluentValidation;
using IdeaTracker.Application.Exceptions;

namespace IdeaTracker.Api.Handlers
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
                await WriteError(context, StatusCodes.Status400BadRequest, "Validation Error", errors);
            }
            catch (NotFoundException ex)
            {
                await WriteError(context, StatusCodes.Status404NotFound, "Not Found", [ex.Message]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception");
                await WriteError(context, StatusCodes.Status500InternalServerError, "Server Error",
                    ["An unexpected error occurred."]);
            }
        }

        private static async Task WriteError(HttpContext context, int statusCode, string title, List<string> errors)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new { Title = title, Errors = errors });
        }
    }
}
