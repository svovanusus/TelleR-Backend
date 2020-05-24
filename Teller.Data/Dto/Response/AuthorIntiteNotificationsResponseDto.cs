using System;
using System.Collections.Generic;

namespace TelleR.Data.Dto.Response
{
    public class AuthorIntiteNotificationsResponseDto
    {
        public IEnumerable<AuthorInviteForReceiverResponseDto> AsReceiver { get; set; }
        public IEnumerable<AuthorInviteNotificationForSenderResponseDto> AsSender { get; set; }
        public String Message { get; set; }
    }
}
