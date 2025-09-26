using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Finsight.Contract.Dto;
using Finsight.Contract.Enum;
using Finsight.Core.Dao;
using Finsight.Core.Dao.Model;
using Finsight.Core.Exceptions;
using Finsight.Core.Extensions;
using Finsight.Core.Mapper;
using Microsoft.Extensions.Logging;

namespace Finsight.Core.Bl.Implementation
{
    public class UserBl : IUserBl
    {
        private readonly IDao<User> _userDao;
        private readonly IDao<Order> _orderDao;
        private readonly ILogger<UserBl> _logger;

        public UserBl(
            IDao<User> userDao,
            IDao<Order> orderDao,
            ILogger<UserBl> logger)
        {
            _userDao = userDao ?? throw new ArgumentNullException(nameof(userDao));
            _orderDao = orderDao ?? throw new ArgumentNullException(nameof(orderDao));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserDto?> LoginAsync(string username, string password, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Неуспешная попытка входа для пользователя {Login}.", username);
                return null;
            }

            var user = await _userDao.FirstOrDefaultAsync(u => u.Login.Equals(username), cancellationToken).ConfigureAwait(false);
            if (user == null)
            {
                var hasAnyUsers = await _userDao.HasAny(cancellationToken).ConfigureAwait(false);
                if (!hasAnyUsers)
                {
                    var initialUser = new UserDto
                    {
                        Login = username,
                        Password = password,
                        DisplayName = username,
                        Role = UserRole.Administrator
                    };
                    var entity = initialUser.ToEntityForCreate();
                    await _userDao.CreateAsync(entity, cancellationToken).ConfigureAwait(false);
                    user = entity;
                    _logger.LogInformation("Во время входа создана учетная запись администратора {Login}.", username);
                }
                else
                {
                    _logger.LogWarning("Неуспешная попытка входа для пользователя {Login}.", username);
                    return null;
                }
            }

            if (!user.PasswordHash.VerifyPassword(password))
            {
                _logger.LogWarning("Неуспешная попытка входа для пользователя {Login}.", username);
                return null;
            }

            _logger.LogInformation("Пользователь {UserId} успешно вошел в систему.", user.Id);
            return user.ToDto();
        }

        public async Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userDao.GetListAsync(null, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Загружено {UserCount} пользователей.", users.Count);
            return users.ToDtos();
        }

        public async Task AddUserAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var duplicate = await _userDao.FirstOrDefaultAsync(u => u.Login.Equals(dto.Login), cancellationToken).ConfigureAwait(false);
            if (duplicate != null)
            {
                _logger.LogWarning("Попытка создать пользователя с существующим логином {Login}.", dto.Login);
                throw new ValidationException("Пользователь с таким именем уже существует.");
            }

            var entity = dto.ToEntityForCreate();
            await _userDao.CreateAsync(entity, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Пользователь {UserId} успешно создан.", entity.Id);
        }

        public async Task UpdateUserAsync(UserDto dto, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var entity = await _userDao.FirstOrDefaultAsync(u => u.Id == dto.Id, cancellationToken).ConfigureAwait(false);
            if (entity == null)
            {
                _logger.LogWarning("Попытка обновить отсутствующего пользователя {UserId}.", dto.Id);
                throw new EntityNotFoundException(nameof(User), dto.Id);
            }

            var duplicate = await _userDao.FirstOrDefaultAsync(u => u.Login.Equals(dto.Login) && u.Id != dto.Id, cancellationToken).ConfigureAwait(false);
            if (duplicate != null)
            {
                _logger.LogWarning("Попытка обновить пользователя {UserId} логином {Login}, который уже используется.", dto.Id, dto.Login);
                throw new ValidationException("Пользователь с таким именем уже существует.");
            }

            dto.ApplyToEntity(entity);
            await _userDao.UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Пользователь {UserId} успешно обновлен.", dto.Id);
        }

        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _userDao.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);
            if (user == null)
            {
                _logger.LogWarning("Попытка удалить отсутствующего пользователя {UserId}.", userId);
                throw new EntityNotFoundException(nameof(User), userId);
            }

            var ordersWithExecutor = await _orderDao
                .GetListAsync(o => o.ExecutorId == userId, cancellationToken)
                .ConfigureAwait(false);

            foreach (var order in ordersWithExecutor)
            {
                order.ExecutorId = null;
                order.Executor = null;
                await _orderDao.UpdateAsync(order, cancellationToken).ConfigureAwait(false);
            }

            await _userDao.DeleteAsync(userId, cancellationToken).ConfigureAwait(false);

            _logger.LogInformation("Пользователь {UserId} успешно удален.", userId);
        }
    }
}
