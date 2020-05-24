using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Contexts;
using TelleR.Data.Entities;
using TelleR.Logic.UnitOfWork;

namespace TelleR.Logic.Repositories.Impl
{
    public class AuthorInviteRepositoryImpl : RepositoryBase<AuthorInvite, AppDbContext>, IAuthorInviteRepository
    {
        #region constructors

        public AuthorInviteRepositoryImpl(UnitOfWorkBase<AppDbContext> uow) : base(uow) { }

        #endregion

        #region public methods

        public async Task<AuthorInvite> Get(Int64 inviteId)
        {
            return await DbSet.Include(x => x.Receiver).Include(x => x.Sender).Include(x => x.Blog).FirstOrDefaultAsync(x => x.Id == inviteId);
        }

        public async Task<IEnumerable<AuthorInvite>> GetAllForBlog(Int64 blogId)
        {
            return await DbSet.Include(x => x.Sender).Include(x => x.Receiver).Include(x => x.Blog).Where(x => x.Blog.Id == blogId).ToArrayAsync();
        }

        public async Task<IEnumerable<AuthorInvite>> GetAllForReceiver(Int64 receiverId)
        {
            return await DbSet.Include(x => x.Sender).Include(x => x.Receiver).Include(x => x.Blog).Where(x => x.Receiver.Id == receiverId && !x.IsApprove.HasValue).ToArrayAsync();
        }

        public async Task<IEnumerable<AuthorInvite>> GetAllForSender(Int64 senderId)
        {
            return await DbSet.Include(x => x.Sender).Include(x => x.Receiver).Include(x => x.Blog).Where(x => x.Sender.Id == senderId && x.IsApprove.HasValue && !x.IsSenderNotified).ToArrayAsync();
        }

        public async Task<AuthorInvite> SaveOrUpdate(AuthorInvite model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                // New

                model.CreateDate = DateTime.Now;
                model.ReceiverRespondDate = null;
                model.SenderViewedDate = null;
                return (await DbSet.AddAsync(model)).Entity;
            }
            else
            {
                // Update

                entity.IsApprove = model.IsApprove;
                entity.IsSenderNotified = model.IsSenderNotified;
                if (entity.IsApprove.HasValue && !entity.ReceiverRespondDate.HasValue) entity.ReceiverRespondDate = DateTime.Now;
                if (entity.IsSenderNotified && !entity.SenderViewedDate.HasValue) entity.SenderViewedDate = DateTime.Now;

                return entity;
            }
        }

        public async Task<User> GetAuthorInviteSender(Int64 inviteId)
        {
            return (await DbSet.Include(x => x.Sender).FirstOrDefaultAsync(x => x.Id == inviteId))?.Sender;
        }

        public async Task<User> GetAuthorInviteReceiver(Int64 inviteId)
        {
            return (await DbSet.Include(x => x.Receiver).FirstOrDefaultAsync(x => x.Id == inviteId))?.Receiver;
        }

        #endregion
    }
}
