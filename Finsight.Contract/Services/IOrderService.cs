using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finsight.Contract.Dto;

namespace Finsight.Contract.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetAll();

        Task Add(OrderDto dto);
        Task Update(OrderDto dto);
        Task Delete(Guid orderId);

        Task<string> UploadDocuments(IEnumerable<string> filePaths);
    }
}
