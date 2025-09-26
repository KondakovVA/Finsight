using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Finsight.WebApi.Infrastructure
{
    public sealed class LoggingJwtBearerEvents : JwtBearerEvents
    {
        private readonly ILogger<LoggingJwtBearerEvents> _logger;

        public LoggingJwtBearerEvents(ILogger<LoggingJwtBearerEvents> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _logger.LogWarning(context.Exception, "Ошибка аутентификации JWT.");
            return base.AuthenticationFailed(context);
        }

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            if (!string.IsNullOrEmpty(context.ErrorDescription))
            {
                _logger.LogWarning("Проверка JWT: {Description}", context.ErrorDescription);
            }

            return base.Challenge(context);
        }
    }
}
