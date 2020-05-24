using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Data.Entities;
using TelleR.Data.Exceptions;
using TelleR.Logic.Repositories;
using TelleR.Logic.Tools;

namespace TelleR.Logic.Services.Impl
{
    public class AuthorInviteServiceImpl : IAuthorInviteService
    {
        #region constructors

        public AuthorInviteServiceImpl(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
        }

        #endregion

        #region public methods

        public async Task<Boolean> SendAuthorInvite(Int64 senderId, Int64 receiverId, Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var blogRepo = uow.GetRepository<IBlogRepository>();

                var sender = await userRepo.GetById(senderId);
                if (sender == null) throw new NotFoundException("Sender not found.");

                var receiver = await userRepo.GetById(receiverId);
                if (receiver == null) throw new NotFoundException("Receiver not found.");

                var blog = await blogRepo.GetById(blogId);
                if (blog == null) throw new NotFoundException("Blog not found.");

                var invite = new AuthorInvite
                {
                    Sender = sender,
                    Receiver = receiver,
                    Blog = blog,
                    IsApprove = null,
                    IsSenderNotified = false,
                };

                var saved = uow.GetRepository<IAuthorInviteRepository>().SaveOrUpdate(invite);

                uow.Commit();

                return saved != null;
            }
        }

        public async Task<Boolean> SendAuthorInvite(Int64 senderId, String receiverEmail, Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var blogRepo = uow.GetRepository<IBlogRepository>();

                var sender = await userRepo.GetById(senderId);
                if (sender == null) throw new NotFoundException("Sender not found.");

                var receiver = await userRepo.GetByEmail(receiverEmail);
                if (receiver == null) throw new NotFoundException("Receiver not found.");

                var blog = await blogRepo.GetById(blogId);
                if (blog == null) throw new NotFoundException("Blog not found.");

                var invite = new AuthorInvite
                {
                    Sender = sender,
                    Receiver = receiver,
                    Blog = blog,
                    IsApprove = null,
                    IsSenderNotified = false,
                };

                var saved = await uow.GetRepository<IAuthorInviteRepository>().SaveOrUpdate(invite);

                uow.Commit();

                return saved != null;
            }
        }

        public async Task<IEnumerable<AuthorInviteForReceiverResponseDto>> GetAuthorInvitesForReceiver(Int64 receiverId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                return (await uow.GetRepository<IAuthorInviteRepository>().GetAllForReceiver(receiverId)).Select(x => new AuthorInviteForReceiverResponseDto
                {
                    InviteId = x.Id,
                    Sender = new UserResponseDto
                    {
                        Id = x.Sender.Id,
                        FullName = $"{ x.Sender.FirstName } { x.Sender.LastName }"
                    },
                    BlogName = x.Blog.Name
                });
            }
        }

        public async Task<IEnumerable<AuthorInviteNotificationForSenderResponseDto>> GetAuthorInviteNotificationForSender(Int64 senderId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                return (await uow.GetRepository<IAuthorInviteRepository>().GetAllForSender(senderId)).Select(x => new AuthorInviteNotificationForSenderResponseDto
                {
                    InviteId = x.Id,
                    Reciever = new UserResponseDto
                    {
                        Id = x.Receiver.Id,
                        FullName = $"{ x.Receiver.FirstName } { x.Receiver.LastName }"
                    },
                    BlogName = x.Blog.Name,
                    IsApproved = x.IsApprove ?? false
                });
            }
        }

        public async Task<Boolean> AnswerToAuthorInvite(Int64 inviteId, Boolean isApprove)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var authorInviteRepo = uow.GetRepository<IAuthorInviteRepository>();

                var invite = await authorInviteRepo.Get(inviteId);
                if (invite == null) throw new NotFoundException("Invite not found.");

                invite.IsApprove = isApprove;

                var saved = await authorInviteRepo.SaveOrUpdate(invite);

                await uow.GetRepository<IBlogRepository>().AddAuthorToBlog(saved.Blog.Id, saved.Receiver.Id);

                uow.Commit();

                return saved != null;
            }
        }

        public async Task<Boolean> CloseAuthorInviteNotification(Int64 inviteId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var authorInviteRepo = uow.GetRepository<IAuthorInviteRepository>();

                var invite = await authorInviteRepo.Get(inviteId);
                if (invite == null) throw new NotFoundException("Invite not found.");

                invite.IsSenderNotified = true;

                var saved = await authorInviteRepo.SaveOrUpdate(invite);

                uow.Commit();

                return saved != null;
            }
        }

        public async Task<Boolean> IsAuthorInviteAvailabeToAnswerForUser(Int64 inviteId, Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var invite = await uow.GetRepository<IAuthorInviteRepository>().Get(inviteId);
                if (invite == null || invite.IsApprove.HasValue) return false;

                var user = await uow.GetRepository<IAuthorInviteRepository>().GetAuthorInviteReceiver(inviteId);
                return user!= null && user.Id == userId;
            }
        }

        public async Task<Boolean> IsAuthorInviteAvailabeToCloseForUser(Int64 inviteId, Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var invite = await uow.GetRepository<IAuthorInviteRepository>().Get(inviteId);
                if (invite == null || !invite.IsApprove.HasValue || invite.IsSenderNotified) return false;

                var user = await uow.GetRepository<IAuthorInviteRepository>().GetAuthorInviteSender(inviteId);
                return user != null && user.Id == userId;
            }
        }

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;

        #endregion
    }
}
