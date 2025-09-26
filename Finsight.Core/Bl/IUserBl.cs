using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Finsight.Contract.Dto;

namespace Finsight.Core.Bl
{
    public interface IUserBl
    {
        Task<UserDto?> LoginAsync(string username, string password, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task AddUserAsync(UserDto dto, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(UserDto dto, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
