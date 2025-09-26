using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Finsight.Contract.Dto;
using Finsight.Core.Dao;
using Finsight.Core.Dao.Model;
using Finsight.Core.Exceptions;
using Finsight.Core.Mapper;
using Microsoft.Extensions.Logging;

namespace Finsight.Core.Bl.Implementation
{
    public class OrderBl : IOrderBl
    {
        private readonly IDao<Order> _orderDao;
        private readonly ILogger<OrderBl> _logger;

        public OrderBl(
            IDao<Order> orderDao,
            ILogger<OrderBl> logger)
        {
            _orderDao = orderDao ?? throw new ArgumentNullException(nameof(orderDao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(OrderDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = dto.ToEntityForCreate();
            await _orderDao.CreateAsync(entity, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Заказ {OrderId} успешно создан.", entity.Id);
        }

        public async Task DeleteAsync(Guid orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderDao.GetByIdAsync(orderId, cancellationToken).ConfigureAwait(false);
            if (order == null)
            {
                _logger.LogWarning("Попытка удалить отсутствующий заказ {OrderId}.", orderId);
                throw new EntityNotFoundException(nameof(Order), orderId);
            }

            await _orderDao.DeleteAsync(orderId, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Заказ {OrderId} успешно удален.", orderId);
        }

        public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var orders = await _orderDao.Join(o => o.Customer)
                .Join(o => o.Executor)
                .GetListAsync(cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Загружено {OrderCount} заказов.", orders.Count);
            return orders.ToDto();
        }

        public async Task UpdateAsync(OrderDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _orderDao.FirstOrDefaultAsync(o => o.Id == dto.Id, cancellationToken).ConfigureAwait(false);
            if (entity == null)
            {
                _logger.LogWarning("Попытка обновить отсутствующий заказ {OrderId}.", dto.Id);
                throw new EntityNotFoundException(nameof(Order), dto.Id);
            }

            dto.ApplyToEntity(entity);
            await _orderDao.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Заказ {OrderId} успешно обновлен.", dto.Id);
        }
    }
}
