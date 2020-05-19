using System;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IPostRepository
    {
        IQueryable<Post> GetAllForBlogQueryable(Int64 blogId);

        Task<Post> SaveOrUpdate(Post post);
    }
}
