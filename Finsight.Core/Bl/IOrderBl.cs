using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Finsight.Contract.Dto;

namespace Finsight.Core.Bl
{
    public interface IOrderBl
    {
        Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default);
        Task AddAsync(OrderDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(OrderDto dto, CancellationToken cancellationToken = default);
    }
}
