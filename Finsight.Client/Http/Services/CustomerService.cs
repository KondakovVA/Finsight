using Finsight.Contract.Dto;
using Finsight.Contract.Services;
using Flurl.Http;

namespace Finsight.Client.Http.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        public async Task<List<CustomerDto>> GetAll()
        {
            return await Client.Request("Customer/GetAll").GetJsonAsync<List<CustomerDto>>();
        }


        public async Task AddCustomer(CustomerDto dto) =>
            await Client.Request("Customer/Add")
                .PostJsonAsync(dto);

        public async Task UpdateCustomer(CustomerDto dto) =>
            await Client.Request("Customer/Update")
                .PutJsonAsync(dto);

        public async Task DeleteCustomer(Guid customerId)
        {
            await Client.Request("Customer/Delete")
                .SetQueryParam(nameof(customerId), customerId)
                .DeleteAsync();
        }
    }
}
