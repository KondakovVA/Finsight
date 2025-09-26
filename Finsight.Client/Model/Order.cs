using Finsight.Contract.Enum;

namespace Finsight.Client.Model
{
    public class Order
    {
        public Guid Id { get; set; }

        public Customer? Customer { get; set; }

        public Executor? Executor { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpireDate { get; set; }

        public OrderStatus Status { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public string DocumentsPath { get; set; } = string.Empty;
    }
}
