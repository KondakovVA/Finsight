using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finsight.Contract.Dto;

namespace Finsight.Contract.Services
{
    public interface IUserService
    {
        Task<UserDto> Login(string username, string password);
        Task<List<UserDto>> GetAllUsers();
        Task AddUser(UserDto dto);
        Task UpdateUser(UserDto dto);
        Task DeleteUser(Guid userId);
    }
}
