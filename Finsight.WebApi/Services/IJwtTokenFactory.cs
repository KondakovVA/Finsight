using Finsight.Contract.Dto;

namespace Finsight.WebApi.Services
{
    public interface IJwtTokenFactory
    {
        string CreateToken(UserDto user);
    }
}
