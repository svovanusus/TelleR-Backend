using System;

namespace TelleR.Data.Dto.Response
{
    public class AuthorInviteNotificationForSenderResponseDto
    {
        public Int64 InviteId { get; set; }
        public UserResponseDto Reciever { get; set; }
        public Boolean IsApproved { get; set; }
        public String BlogName { get; set; }
    }
}
