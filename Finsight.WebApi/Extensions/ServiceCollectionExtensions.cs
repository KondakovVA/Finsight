using Finsight.WebApi.Options;
using Finsight.WebApi.Services;

namespace Finsight.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            services.AddOptions<OrderDocumentOptions>()
                .Bind(configuration.GetSection(OrderDocumentOptions.SectionName))
                .ValidateDataAnnotations()
                .Validate(options => options.AllowedExtensions.Count > 0, "Необходимо указать хотя бы одно допустимое расширение файлов.")
                .ValidateOnStart();

            services.AddScoped<IJwtTokenFactory, JwtTokenFactory>();
            services.AddScoped<IOrderDocumentService, OrderDocumentService>();

            return services;
        }
    }
}
