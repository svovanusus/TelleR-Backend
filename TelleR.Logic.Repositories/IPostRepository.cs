using System;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IPostRepository
    {
        Task<Post> Get(Int64 postId);

        IQueryable<Post> GetAllForBlogQueryable(Int64 blogId);

        Task<Post> SaveOrUpdate(Post post);

        Task<Blog> GetPostBlog(Int64 postId);
    }
}
