using Finsight.Contract.Enum;

namespace Finsight.Core.Dao.Model
{
    public class User : EntityData
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; } = string.Empty;
        /// <summary>
        /// Пароль (хранится в виде хэша)
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;
        /// <summary>
        /// Роль пользователя
        /// </summary>
        public UserRole Role { get; set; }
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
    }
}
