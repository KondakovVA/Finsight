namespace Finsight.Contract.Dto
{
    public class AuthResponseDto : BaseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }
}
