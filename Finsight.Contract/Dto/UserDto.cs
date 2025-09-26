using Finsight.Contract.Enum;

namespace Finsight.Contract.Dto
{
    public class UserDto : BaseDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public UserRole Role { get; set; }
    }
}
