using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IAuthorInviteRepository
    {
        Task<AuthorInvite> Get(Int64 inviteId);

        Task<User> GetAuthorInviteSender(Int64 inviteId);

        Task<User> GetAuthorInviteReceiver(Int64 inviteId);

        Task<IEnumerable<AuthorInvite>> GetAllForReceiver(Int64 receiverId);

        Task<IEnumerable<AuthorInvite>> GetAllForSender(Int64 senderId);

        Task<IEnumerable<AuthorInvite>> GetAllForBlog(Int64 blogId);

        Task<AuthorInvite> SaveOrUpdate(AuthorInvite entity);
    }
}
