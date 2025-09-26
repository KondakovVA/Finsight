using System;
using Finsight.Contract.Enum;

namespace Finsight.Contract.Dto
{
    public class OrderDto : BaseDto
    {
        public Guid CustomerId { get; set; }
        public string CustomerCompany { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public Guid? ExecutorId { get; set; }
        public string ExecutorName { get; set; }
        public OrderStatus Status { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public string DocumentsPath { get; set; }
    }
}
