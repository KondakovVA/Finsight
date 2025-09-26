using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Finsight.Contract.Dto;
using Finsight.WebApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Finsight.WebApi.Services
{
    public sealed class JwtTokenFactory : IJwtTokenFactory
    {
        private readonly IOptionsMonitor<JwtOptions> _optionsMonitor;
        private readonly ILogger<JwtTokenFactory> _logger;

        public JwtTokenFactory(IOptionsMonitor<JwtOptions> optionsMonitor, ILogger<JwtTokenFactory> logger)
        {
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string CreateToken(UserDto user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var options = _optionsMonitor.CurrentValue;
            if (!options.TryGetAuthTokenBytes(out var keyBytes))
            {
                _logger.LogError("Ключ подписи JWT настроен некорректно.");
                throw new InvalidOperationException("Ключ подписи JWT настроен некорректно.");
            }

            var signingKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Login) }),
                Expires = DateTime.UtcNow.AddMinutes(options.TokenLifetimeMinutes),
                SigningCredentials = credentials,
                Audience = options.Audience,
                Issuer = options.Issuer
            };

            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);

            _logger.LogInformation("Сгенерирован JWT-токен для пользователя [{UserLogin}]{UserId}.", user.Login, user.Id);
            return handler.WriteToken(token);
        }
    }
}
