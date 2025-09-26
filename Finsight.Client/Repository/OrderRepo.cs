using Finsight.Client.DI;
using Finsight.Client.Mappers;
using Finsight.Client.Model;
using Finsight.Contract.Services;

namespace Finsight.Client.Repository
{
    public class OrderRepo : IOrderRepo
    {
        private readonly IOrderService _orderService;

        public OrderRepo(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<Order>> GetAll()
        {
            var dtos = await _orderService.GetAll();
            return dtos.FromDtos();
        }

        public async Task AddOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            var dto = order.ToDto();
            await _orderService.Add(dto);
        }

        public async Task UpdateOrder(Order order)
        {
            ArgumentNullException.ThrowIfNull(order);

            var dto = order.ToDto();
            await _orderService.Update(dto);
        }

        public async Task<string> UploadDocuments(IEnumerable<string> filePaths)
        {
            ArgumentNullException.ThrowIfNull(filePaths);

            return await _orderService.UploadDocuments(filePaths);
        }

        public async Task Delete(Guid id)
        {
            await _orderService.Delete(id);
        }
    }
}
