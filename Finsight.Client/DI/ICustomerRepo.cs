using Finsight.Client.Model;

namespace Finsight.Client.DI
{
    public interface ICustomerRepo
    {
        Task<List<Customer>> GetAll();
        Task AddCustomer(Customer customer);
        Task UpdateCustomer(Customer customer);
        Task DeleteCustomer(Guid customerId);
    }
}
