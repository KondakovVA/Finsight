using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using Finsight.WebApi.Options;

namespace Finsight.WebApi.Infrastructure.Security
{
    public sealed class JwtAuthTokenStore
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public JwtAuthTokenStore(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public JwtOptions EnsureToken()
        {
            var jwtSection = _configuration.GetSection(JwtOptions.SectionName);
            var jwtOptions = jwtSection.Get<JwtOptions>() ?? new JwtOptions();

            if (jwtOptions.TryGetAuthTokenBytes(out _))
            {
                return jwtOptions;
            }

            var newToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            jwtSection[nameof(JwtOptions.AuthToken)] = newToken;
            PersistToken(newToken);
            jwtOptions.AuthToken = newToken;

            return jwtOptions;
        }

        private void PersistToken(string token)
        {
            var appsettingsPath = Path.Combine(_environment.ContentRootPath, "appsettings.json");
            JsonObject rootObject;

            if (File.Exists(appsettingsPath))
            {
                var json = File.ReadAllText(appsettingsPath);
                rootObject = JsonNode.Parse(json) as JsonObject ?? new JsonObject();
            }
            else
            {
                rootObject = new JsonObject();
            }

            if (rootObject[JwtOptions.SectionName] is not JsonObject jwtSection)
            {
                jwtSection = new JsonObject();
                rootObject[JwtOptions.SectionName] = jwtSection;
            }

            jwtSection[nameof(JwtOptions.AuthToken)] = token;

            var serializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            File.WriteAllText(appsettingsPath, rootObject.ToJsonString(serializerOptions));
        }
    }
}
