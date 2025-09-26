using Finsight.Contract.Dto;
using Finsight.Core.Bl;
using Finsight.Core.Exceptions;
using Finsight.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finsight.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderBl _orderBl;
        private readonly IOrderDocumentService _documentService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderBl orderBl, IOrderDocumentService documentService, ILogger<OrderController> logger)
        {
            _orderBl = orderBl ?? throw new ArgumentNullException(nameof(orderBl));
            _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<List<OrderDto>>> GetAll(CancellationToken cancellationToken)
        {
            var orders = await _orderBl.GetAllAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Возвращено {OrderCount} заказов.", orders.Count);
            return Ok(orders);
        }

        [HttpPost(nameof(Add))]
        public async Task<ActionResult> Add([FromBody] OrderDto dto, CancellationToken cancellationToken)
        {
            await _orderBl.AddAsync(dto, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Заказ {OrderId} создан через API.", dto.Id);
            return Ok();
        }

        [HttpPut(nameof(Update))]
        public async Task<ActionResult> Update([FromBody] OrderDto dto, CancellationToken cancellationToken)
        {
            await _orderBl.UpdateAsync(dto, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Заказ {OrderId} обновлен через API.", dto.Id);
            return Ok();
        }

        [HttpDelete(nameof(Delete))]
        public async Task<ActionResult> Delete(Guid orderId, CancellationToken cancellationToken)
        {
            await _orderBl.DeleteAsync(orderId, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Заказ {OrderId} удален через API.", orderId);
            return Ok();
        }

        [HttpPost(nameof(UploadDocuments))]
        public async Task<ActionResult<string>> UploadDocuments([FromForm] List<IFormFile> files, CancellationToken cancellationToken)
        {
            var username = User?.Identity?.Name ?? string.Empty;

            try
            {
                var relativePath = await _documentService.SaveAsync(username, files, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("Документы для пользователя {User} загружены.", username);
                return Ok(relativePath);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Не удалось загрузить документы для пользователя {User}.", username);
                return BadRequest(ex.Message);
            }
        }
    }
}
