using Finsight.Client.Model;
using Finsight.Contract.Dto;

namespace Finsight.Client.Mappers
{
    public static class OrderMapper
    {
        public static Order FromDto(this OrderDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new Order
            {
                Id = dto.Id,
                Customer = dto.CustomerId == Guid.Empty && string.IsNullOrWhiteSpace(dto.CustomerCompany)
                    ? null
                    : new Customer
                    {
                        Id = dto.CustomerId,
                        CompanyName = dto.CustomerCompany ?? string.Empty
                    },
                Executor = dto.ExecutorId.HasValue || !string.IsNullOrWhiteSpace(dto.ExecutorName)
                    ? new Executor
                    {
                        Id = dto.ExecutorId,
                        DisplayName = dto.ExecutorName ?? string.Empty
                    }
                    : null,
                StartDate = dto.StartDate,
                ExpireDate = dto.ExpireDate,
                Status = dto.Status,
                Price = dto.Price,
                Description = dto.Description ?? string.Empty,
                Comment = dto.Comment ?? string.Empty,
                DocumentsPath = dto.DocumentsPath ?? string.Empty
            };
        }

        public static List<Order> FromDtos(this List<OrderDto>? dtos)
        {
            return dtos?.Select(FromDto).ToList() ?? new List<Order>();
        }

        public static OrderDto ToDto(this Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            return new OrderDto
            {
                Id = order.Id,
                CustomerId = order.Customer?.Id ?? Guid.Empty,
                CustomerCompany = order.Customer?.CompanyName,
                ExecutorId = order.Executor?.Id,
                ExecutorName = order.Executor?.DisplayName,
                StartDate = order.StartDate,
                ExpireDate = order.ExpireDate,
                Status = order.Status,
                Price = order.Price,
                Description = order.Description,
                Comment = order.Comment,
                DocumentsPath = order.DocumentsPath
            };
        }

        public static List<OrderDto> ToDtos(this IEnumerable<Order> orders)
        {
            ArgumentNullException.ThrowIfNull(orders);
            return orders.Select(ToDto).ToList();
        }
    }
}
