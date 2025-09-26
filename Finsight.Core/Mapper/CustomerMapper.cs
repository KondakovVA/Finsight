using System;
using System.Collections.Generic;
using System.Linq;
using Finsight.Contract.Dto;
using Finsight.Core.Dao.Model;

namespace Finsight.Core.Mapper
{
    public static class CustomerMapper
    {
        public static CustomerDto ToDto(this Customer entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return new CustomerDto
            {
                Id = entity.Id,
                ContactName = entity.ContactName,
                CompanyName = entity.CompanyName,
                Email = entity.Email,
                Phone = entity.Phone,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate
            };
        }
        public static List<CustomerDto> ToDto(this IEnumerable<Customer> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);
            return entities.Select(ToDto).ToList();
        }

        public static Customer ToEntityForCreate(this CustomerDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            return new Customer
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                ContactName = dto.ContactName ?? string.Empty,
                CompanyName = dto.CompanyName ?? string.Empty,
                Email = dto.Email ?? string.Empty,
                Phone = dto.Phone ?? string.Empty,
                Status = dto.Status,
                CreatedDate = dto.CreatedDate == default ? DateTime.UtcNow : dto.CreatedDate
            };
        }

        public static void ApplyToEntity(this CustomerDto dto, Customer entity)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            entity.ContactName = dto.ContactName ?? string.Empty;
            entity.CompanyName = dto.CompanyName ?? string.Empty;
            entity.Email = dto.Email ?? string.Empty;
            entity.Phone = dto.Phone ?? string.Empty;
            entity.Status = dto.Status;
        }
    }
}
