using System;

namespace TelleR.Data.Dto.Response
{
    public class UserInfoForEditResponseDto
    {
        public Int64 UserId { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
    }
}
