using Finsight.Contract.Enum;
using System;

namespace Finsight.Core.Dao.Model
{
    public class Customer : EntityData
    {
        /// <summary>
        /// Название компании
        /// </summary>
        public string CompanyName { get; set; } = string.Empty;
        /// <summary>
        /// Контактное лицо
        /// </summary>
        public string ContactName { get; set; } = string.Empty;
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Контактный телефон
        /// </summary>
        public string Phone { get; set; } = string.Empty;
        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Статус клиента
        /// </summary>
        public CustomerStatus Status { get; set; }
    }
}
