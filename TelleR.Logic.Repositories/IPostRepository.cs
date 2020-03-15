using System;
using System.Linq;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IPostRepository
    {
        IQueryable<Post> GetAllForBlog(Int64 blogId);
    }
}
