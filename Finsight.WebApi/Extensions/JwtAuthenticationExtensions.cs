using System;
using Finsight.WebApi.Infrastructure;
using Finsight.WebApi.Infrastructure.Security;
using Finsight.WebApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Finsight.WebApi.Extensions
{
    public static class JwtAuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentNullException.ThrowIfNull(environment);

            var tokenStore = new JwtAuthTokenStore(configuration, environment);
            var jwtOptions = tokenStore.EnsureToken();

            services.AddOptions<JwtOptions>()
                .Bind(configuration.GetSection(JwtOptions.SectionName))
                .ValidateDataAnnotations()
                .Validate(options => options.TryGetAuthTokenBytes(out _), "Jwt:AuthToken должен быть корректной строкой Base64.")
                .ValidateOnStart();

            if (!jwtOptions.TryGetAuthTokenBytes(out var keyBytes))
            {
                throw new InvalidOperationException("Не удалось инициализировать ключ подписи JWT.");
            }

            services.AddScoped<LoggingJwtBearerEvents>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.EventsType = typeof(LoggingJwtBearerEvents);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    };
                });

            return services;
        }
    }
}
