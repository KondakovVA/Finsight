using Finsight.Contract.Dto;
using Finsight.Core.Bl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finsight.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerBl _customerBl;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerBl customerBl, ILogger<CustomerController> logger)
        {
            _customerBl = customerBl ?? throw new ArgumentNullException(nameof(customerBl));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<List<CustomerDto>>> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerBl.GetAllAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Возвращено {CustomerCount} клиентов.", customers.Count);
            return Ok(customers);
        }

        [HttpPost(nameof(Add))]
        public async Task<ActionResult> Add([FromBody] CustomerDto dto, CancellationToken cancellationToken)
        {
            await _customerBl.AddAsync(dto, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Клиент {Company} создан через API.", dto.CompanyName);
            return Ok();
        }

        [HttpPut(nameof(Update))]
        public async Task<ActionResult> Update([FromBody] CustomerDto dto, CancellationToken cancellationToken)
        {
            await _customerBl.UpdateAsync(dto, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Клиент {CustomerId} обновлен через API.", dto.Id);
            return Ok();
        }

        [HttpDelete(nameof(Delete))]
        public async Task<ActionResult> Delete(Guid customerId, CancellationToken cancellationToken)
        {
            await _customerBl.DeleteAsync(customerId, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Клиент {CustomerId} удален через API.", customerId);
            return Ok();
        }
    }
}
