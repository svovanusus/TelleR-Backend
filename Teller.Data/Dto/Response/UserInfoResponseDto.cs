using System;
using TelleR.Data.Enums;

namespace TelleR.Data.Dto.Response
{
    public class UserInfoResponseDto
    {
        public Int64 UserId { get; set; }
        public String FullName { get; set; }
        public String Avatar { get; set; }
        public UserRole Role { get; set; }
    }
}
