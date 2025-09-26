using System;
using Finsight.Contract.Enum;

namespace Finsight.Contract.Dto
{
    public class CustomerDto : BaseDto
    {
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedDate { get; set; }
        public CustomerStatus Status { get; set; }
    }
}
