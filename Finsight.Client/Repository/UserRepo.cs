using Finsight.Client.DI;
using Finsight.Contract.Dto;
using Finsight.Contract.Services;

namespace Finsight.Client.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly IUserService _userService;

        public UserRepo(IUserService userService)
        {
            _userService = userService;
        }

        public Task<UserDto> Login(string username, string password)
        {
            return _userService.Login(username, password);
        }

        public Task AddUser(UserDto dto)
        {
            return _userService.AddUser(dto);
        }

        public Task UpdateUser(UserDto dto)
        {
            return _userService.UpdateUser(dto);
        }

        public Task DeleteUser(Guid userId)
        {
            return _userService.DeleteUser(userId);
        }

        public Task<List<UserDto>> GetAllUsers()
        {
            return _userService.GetAllUsers();
        }
    }
}
