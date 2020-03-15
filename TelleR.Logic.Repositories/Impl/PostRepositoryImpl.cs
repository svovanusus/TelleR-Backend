using System.Linq;
using TelleR.Data.Contexts;
using TelleR.Data.Entities;
using TelleR.Logic.UnitOfWork;

namespace TelleR.Logic.Repositories.Impl
{
    public class PostRepositoryImpl : RepositoryBase<Post, AppDbContext>, IPostRepository
    {
        public PostRepositoryImpl(UnitOfWorkBase<AppDbContext> uow) : base(uow) { }

        public IQueryable<Post> GetAllForBlog(long blogId)
        {
            return DbSet.Where(x => x.Blog.Id == blogId);
        }
    }
}
