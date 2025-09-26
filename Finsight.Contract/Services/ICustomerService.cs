using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finsight.Contract.Dto;

namespace Finsight.Contract.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAll();
        Task AddCustomer(CustomerDto dto);
        Task UpdateCustomer(CustomerDto dto);
        Task DeleteCustomer(Guid customerId);
    }
}
