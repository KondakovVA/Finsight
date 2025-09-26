using Finsight.Client.DI;
using Finsight.Client.Mappers;
using Finsight.Client.Model;
using Finsight.Contract.Services;

namespace Finsight.Client.Repository
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ICustomerService _customerService;
        public CustomerRepo(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<List<Customer>> GetAll()
        {
            var dtos = await _customerService.GetAll();
            return dtos.FromDtos();
        }

        public async Task AddCustomer(Customer customer)
        {
            var dto = customer.ToDto();
            await _customerService.AddCustomer(dto);
        }

        public async Task UpdateCustomer(Customer customer)
        {
            var dto = customer.ToDto();
            await _customerService.UpdateCustomer(dto);
        }

        public async Task DeleteCustomer(Guid customerId)
        {
            await _customerService.DeleteCustomer(customerId);
        }
    }
}
