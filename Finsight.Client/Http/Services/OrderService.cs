using System.IO;
using Finsight.Contract.Dto;
using Finsight.Contract.Services;
using Flurl.Http;

namespace Finsight.Client.Http.Services
{
    internal class OrderService : BaseService, IOrderService
    {
        public async Task<List<OrderDto>> GetAll()
        {
            return await Client.Request("Order/GetAll").GetJsonAsync<List<OrderDto>>();
        }

        public async Task Add(OrderDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            await Client.Request("Order/Add").PostJsonAsync(dto);
        }

        public async Task Update(OrderDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            await Client.Request("Order/Update").PutJsonAsync(dto);
        }

        public async Task<string> UploadDocuments(IEnumerable<string> filePaths)
        {
            ArgumentNullException.ThrowIfNull(filePaths);

            var files = filePaths.Where(File.Exists).ToList();
            if (files.Count == 0)
            {
                throw new ArgumentException("Не выбрано ни одного файла для загрузки.");
            }

            var response = await Client.Request("Order/UploadDocuments").PostMultipartAsync(content =>
            {
                foreach (var filePath in files)
                {
                    content.AddFile("files", filePath, Path.GetFileName(filePath));
                }
            });

            return await response.GetStringAsync();
        }

        public async Task Delete(Guid orderId)
        {
             await Client.Request("Order/Delete").
                SetQueryParam(nameof(orderId), orderId)
                .DeleteAsync();
        }
    }
}
