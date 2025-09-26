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
    public class CustomerBl : ICustomerBl
    {
        private readonly IDao<Customer> _customerDao;
        private readonly ILogger<CustomerBl> _logger;

        public CustomerBl(
            IDao<Customer> customerDao,
            ILogger<CustomerBl> logger)
        {
            _customerDao = customerDao ?? throw new ArgumentNullException(nameof(customerDao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<CustomerDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var customers = await _customerDao
                .GetListAsync(null, cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Загружено {CustomerCount} клиентов.", customers.Count);
            return customers.ToDto();
        }

        public async Task DeleteAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            var customer = await _customerDao.FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken).ConfigureAwait(false);
            if (customer == null)
            {
                _logger.LogWarning("Попытка удалить отсутствующего клиента {CustomerId}.", customerId);
                throw new EntityNotFoundException(nameof(Customer), customerId);
            }

            await _customerDao.DeleteAsync(customerId, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Клиент {CustomerId} успешно удален.", customerId);
        }

        public async Task AddAsync(CustomerDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = dto.ToEntityForCreate();
            await _customerDao.CreateAsync(entity, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Клиент {CustomerId} успешно создан.", entity.Id);
        }

        public async Task UpdateAsync(CustomerDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _customerDao
                .FirstOrDefaultAsync(c => c.Id == dto.Id, cancellationToken)
                .ConfigureAwait(false);

            if (entity == null)
            {
                _logger.LogWarning("Попытка обновить отсутствующего клиента {CustomerId}.", dto.Id);
                throw new EntityNotFoundException(nameof(Customer), dto.Id);
            }

            dto.ApplyToEntity(entity);
            await _customerDao.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Клиент {CustomerId} успешно обновлен.", dto.Id);
        }
    }
}
