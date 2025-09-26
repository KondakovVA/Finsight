using Finsight.Contract.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Finsight.Core.Bl
{
    public interface ICustomerBl
    {
        Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid customerId, CancellationToken cancellationToken = default);
        Task AddAsync(CustomerDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(CustomerDto dto, CancellationToken cancellationToken = default);
    }
}
