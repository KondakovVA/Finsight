using Finsight.Contract.Dto;
using Finsight.Contract.Services;
using Flurl.Http;
using Newtonsoft.Json;

namespace Finsight.Client.Http.Services
{
    public class UserService : BaseService, IUserService
    {
        public async Task<UserDto> Login(string username, string password)
        {
            var response = await Client.Request("User/Login")
                .SetQueryParam(nameof(username), username)
                .SetQueryParam(nameof(password), password)
                .PostJsonAsync(new { username, password });

            if (!response.ResponseMessage.IsSuccessStatusCode) return null;

            var content = await response.ResponseMessage.Content.ReadAsStringAsync();
            var authDto = JsonConvert.DeserializeObject<AuthResponseDto>(content);
            if (authDto == null) return null;
            SetAuthHeader(authDto.Token);
            return authDto.User;
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            return await Client.Request("User/GetAllUsers").GetJsonAsync<List<UserDto>>();
        }

        public async Task AddUser(UserDto dto)
        {
            await Client.Request("User/AddUser").PostJsonAsync(dto);
        }

        public async Task UpdateUser(UserDto dto)
        {
            await Client.Request("User/UpdateUser").PutJsonAsync(dto);
        }

        public async Task DeleteUser(Guid userId)
        {
            await Client.Request("User/DeleteUser").AppendQueryParam(nameof(userId), userId).GetAsync();
        }
    }
}
