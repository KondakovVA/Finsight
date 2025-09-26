namespace Finsight.Client.Utils
{
    public static class CredentialsValidator
    {
        private static readonly int MinLoginLenght = 4;
        private static readonly int MinPasswordLenght = 8;
        public static (bool,string) Validate(string? login, string password)
        {
            var result = true;
            var errors = new List<string>();

            if (login == null || login.Length < MinLoginLenght)
                errors.Add($"Длина логина не может быть меньше {MinLoginLenght} знаков!");
            if (password == null || password.Length < MinPasswordLenght)
                errors.Add($"Длина пароля не может быть меньше {MinPasswordLenght} знаков!");

            if (errors.Any())
                return (false, string.Join("\n", errors));

            return (result, string.Empty);
        }
    }
}
