using Finsight.Contract.Dto;

namespace Finsight.Client.DI
{
    public interface IUserRepo
    {
        Task<UserDto> Login(string username, string password);
        Task AddUser(UserDto dto);
        Task UpdateUser(UserDto dto);
        Task DeleteUser(Guid userId);
        Task<List<UserDto>> GetAllUsers();
    }
}
