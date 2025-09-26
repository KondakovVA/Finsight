using System;

namespace Finsight.Core.Extensions
{
    public static class BcryptExtension
    {
        public static string HashPassword(this string password)
        {
            ArgumentNullException.ThrowIfNull(password);
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(this string hashedPassword, string password)
        {
            ArgumentNullException.ThrowIfNull(hashedPassword);
            ArgumentNullException.ThrowIfNull(password);
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
