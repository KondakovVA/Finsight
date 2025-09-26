using Finsight.Contract.Dto;

namespace Finsight.Client.Services
{
    public interface IUserSession
    {
        UserDto CurrentUser { get; }
        bool IsAuthenticated { get; }
        void SetUser(UserDto user);
        void Clear();
    }
}
