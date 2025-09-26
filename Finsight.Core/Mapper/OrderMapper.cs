using System;
using System.Collections.Generic;
using System.Linq;
using Finsight.Contract.Dto;
using Finsight.Core.Dao.Model;

namespace Finsight.Core.Mapper
{
    public static class OrderMapper
    {
        public static OrderDto ToDto(this Order entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new OrderDto
            {
                Id = entity.Id,
                CustomerId = entity.CustomerId,
                CustomerCompany = entity.Customer?.CompanyName ?? string.Empty,
                ExecutorId = entity.ExecutorId,
                ExecutorName = entity.Executor?.DisplayName ?? string.Empty,
                StartDate = entity.StartDate,
                ExpireDate = entity.ExpireDate,
                Status = entity.Status,
                Comment = entity.Comment,
                Description = entity.Description,
                Price = entity.Price,
                DocumentsPath = entity.DocumentsPath
            };
        }

        public static List<OrderDto> ToDto(this IEnumerable<Order> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            return entities.Select(ToDto).ToList();
        }

        public static Order ToEntityForCreate(this OrderDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Order
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                CustomerId = dto.CustomerId,
                ExecutorId = dto.ExecutorId,
                StartDate = dto.StartDate,
                ExpireDate = dto.ExpireDate,
                Status = dto.Status,
                Comment = dto.Comment ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                Price = dto.Price,
                DocumentsPath = dto.DocumentsPath ?? string.Empty
            };
        }

        public static void ApplyToEntity(this OrderDto dto, Order entity)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.CustomerId = dto.CustomerId;
            entity.ExecutorId = dto.ExecutorId;
            entity.StartDate = dto.StartDate;
            entity.ExpireDate = dto.ExpireDate;
            entity.Status = dto.Status;
            entity.Comment = dto.Comment ?? string.Empty;
            entity.Description = dto.Description ?? string.Empty;
            entity.Price = dto.Price;
            entity.DocumentsPath = dto.DocumentsPath ?? string.Empty;
        }
    }
}
