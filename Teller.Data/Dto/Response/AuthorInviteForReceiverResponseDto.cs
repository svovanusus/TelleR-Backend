using System;

namespace TelleR.Data.Dto.Response
{
    public class AuthorInviteForReceiverResponseDto
    {
        public Int64 InviteId { get; set; }
        public UserResponseDto Sender { get; set; }
        public String BlogName { get; set; }
    }
}
