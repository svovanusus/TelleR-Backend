using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;

namespace TelleR.Logic.Services
{
    public interface IAuthorInviteService
    {
        Task<Boolean> SendAuthorInvite(Int64 senderId, Int64 receiverId, Int64 blogId);

        Task<Boolean> SendAuthorInvite(Int64 senderId, String receiverEmail, Int64 blogId);

        Task<IEnumerable<AuthorInviteForReceiverResponseDto>> GetAuthorInvitesForReceiver(Int64 receiverId);

        Task<IEnumerable<AuthorInviteNotificationForSenderResponseDto>> GetAuthorInviteNotificationForSender(Int64 senderId);

        Task<Boolean> AnswerToAuthorInvite(Int64 inviteId, Boolean isApprove);

        Task<Boolean> CloseAuthorInviteNotification(Int64 infiteId);

        Task<Boolean> IsAuthorInviteAvailabeToAnswerForUser(Int64 inviteId, Int64 userId);

        Task<Boolean> IsAuthorInviteAvailabeToCloseForUser(Int64 inviteId, Int64 userId);
    }
}
