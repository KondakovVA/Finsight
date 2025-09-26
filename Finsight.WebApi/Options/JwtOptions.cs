using System.ComponentModel.DataAnnotations;

namespace Finsight.WebApi.Options
{
    public sealed class JwtOptions
    {
        public const string SectionName = "Jwt";

        [Required]
        public string AuthToken { get; set; } = string.Empty;

        [Required]
        public string Issuer { get; set; } = string.Empty;

        [Required]
        public string Audience { get; set; } = string.Empty;

        [Range(1, 1440)]
        public int TokenLifetimeMinutes { get; set; } = 60;

        public bool TryGetAuthTokenBytes(out byte[] key)
        {
            if (string.IsNullOrWhiteSpace(AuthToken))
            {
                key = Array.Empty<byte>();
                return false;
            }

            var buffer = new byte[AuthToken.Length];
            if (Convert.TryFromBase64String(AuthToken, buffer, out var bytesWritten))
            {
                key = new byte[bytesWritten];
                Array.Copy(buffer, key, bytesWritten);
                return true;
            }

            key = Array.Empty<byte>();
            return false;
        }
    }
}
