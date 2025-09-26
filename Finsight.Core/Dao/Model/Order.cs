using System;
using Finsight.Contract.Enum;

namespace Finsight.Core.Dao.Model
{
    public class Order : EntityData
    {
        /// <summary>
        /// Клиент
        /// </summary>
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Ожидаемая дата выполнения заказа
        /// </summary>
        public DateTime ExpireDate { get; set; }
        /// <summary>
        /// Исполнитель - пользователь
        /// </summary>
        public Guid? ExecutorId { get; set; }
        public User? Executor { get; set; }
        /// <summary>
        /// Статус заказа
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// Стоимость заказа
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Описание заказа
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Комментарий к заказу
        /// </summary>
        public string Comment { get; set; } = string.Empty;
        /// <summary>
        /// Путь к документам заказа
        /// </summary>
        public string DocumentsPath { get; set; } = string.Empty;
    }
}
