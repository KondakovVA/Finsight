using System.Text.Json;
using Finsight.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Finsight.WebApi.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context).ConfigureAwait(false);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Ошибка валидации при обработке запроса {Path}.", context.Request.Path);
                await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, "Ошибка валидации", ex.Message).ConfigureAwait(false);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogWarning(ex, "Сущность не найдена при обработке запроса {Path}.", context.Request.Path);
                await WriteProblemDetailsAsync(context, StatusCodes.Status404NotFound, "Сущность не найдена", ex.Message).ConfigureAwait(false);
            }
            catch (FinsightException ex)
            {
                _logger.LogError(ex, "Бизнес-ошибка при обработке запроса {Path}.", context.Request.Path);
                await WriteProblemDetailsAsync(context, StatusCodes.Status400BadRequest, "Бизнес-ошибка", ex.Message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Необработанное исключение при обработке запроса {Path}.", context.Request.Path);
                await WriteProblemDetailsAsync(context, StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера", "Произошла непредвиденная ошибка.").ConfigureAwait(false);
            }
        }

        private static Task WriteProblemDetailsAsync(HttpContext context, int statusCode, string title, string detail)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var problem = new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = statusCode,
                Instance = context.Request.Path
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(problem, SerializerOptions));
        }
    }
}
