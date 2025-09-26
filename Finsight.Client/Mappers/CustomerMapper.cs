using Finsight.Client.Model;
using Finsight.Contract.Dto;

namespace Finsight.Client.Mappers
{
    public static class CustomerMapper
    {
        public static Customer FromDto(this CustomerDto dto)
        {
            return new Customer
            {
                Id = dto.Id,
                ContactName = dto.ContactName,
                CompanyName = dto.CompanyName,
                Email = dto.Email,
                Phone = dto.Phone,
                User = dto.User,
                UserId = dto.UserId,
                CreatedDate = dto.CreatedDate,
                Status = dto.Status
            };
        }

        public static List<Customer> FromDtos(this List<CustomerDto> dtos)
        {
            return dtos?.Select(FromDto)?.ToList();
        }

        public static CustomerDto ToDto(this Customer entity)
        {
            return new CustomerDto
            {
                Id = entity.Id,
                ContactName = entity.ContactName,
                CompanyName = entity.CompanyName,
                Email = entity.Email,
                Phone = entity.Phone,
                User = entity.User,
                UserId = entity.UserId,
                CreatedDate = entity.CreatedDate,
                Status = entity.Status
            };
        }
    }
}
