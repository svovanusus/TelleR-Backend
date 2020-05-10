using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Repositories;
using TelleR.Logic.Tools;

namespace TelleR.Logic.Services.Impl
{
    public class BlogServiceImpl : IBlogService
    {
        #region constructors

        public BlogServiceImpl(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory, IAwsService awsService)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
            _awsService = awsService;
        }

        #endregion

        #region public methods

        public async Task<BlogResponseDto> GetById(long blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetByIdWithOwner(blogId);

                if (blog == null) return null;

                return new BlogResponseDto
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Author = new UserResponseDto
                    {
                        Id = blog.Owner.Id,
                        FullName = $"{ blog.Owner.FirstName } { blog.Owner.LastName }"
                    }
                };
            }
        }

        public async Task<BlogResponseDto> GetByName(string blogName)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetByName(blogName);

                if (blog == null) return null;

                return new BlogResponseDto
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Author = new UserResponseDto
                    {
                        Id = blog.Owner.Id,
                        FullName = $"{ blog.Owner.FirstName } { blog.Owner.LastName }"
                    }
                };
            }
        }

        public async Task<IEnumerable<BlogResponseDto>> GetAll(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blogs = await uow.GetRepository<IBlogRepository>().GetAllByOwner(userId);

                return blogs.Select(x => new BlogResponseDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = new UserResponseDto
                    {
                        Id = x.Owner.Id,
                        FullName = $"{ x.Owner.FirstName } { x.Owner.LastName }"
                    }
                });
            }
        }

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;
        private readonly IAwsService _awsService;

        #endregion
    }
}
