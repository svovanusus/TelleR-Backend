using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Repositories;
using TelleR.Logic.Tools;

namespace TelleR.Logic.Services.Impl
{
    public class BlogServiceImpl : IBlogService
    {
        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;
        private readonly IAwsService _awsService;

        public BlogServiceImpl(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory, IAwsService awsService)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
            _awsService = awsService;
        }

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
    }
}
