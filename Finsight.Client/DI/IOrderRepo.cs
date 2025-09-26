using Finsight.Client.Model;

namespace Finsight.Client.DI
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAll();

        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task Delete(Guid id);

        Task<string> UploadDocuments(IEnumerable<string> filePaths);
    }
}
