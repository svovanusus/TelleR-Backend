using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IBlogRepository
    {
        ValueTask<Blog> GetById(Int64 blogId);

        Task<Blog> GetByIdWithOwner(Int64 blogId);

        Task<Blog> GetByName(String blogName);

        Task<IEnumerable<Blog>> GetAll();

        Task<IEnumerable<Blog>> GetAllByOwner(Int64 userId);

        Task<IEnumerable<Blog>> GetAllWithPostsByAuthor(Int64 userId);

        Task<IEnumerable<Int64>> GetAuthors(Int64 blogId);

        Task AddAuthorToBlog(Int64 blogId, Int64 authorId);

        Task RemoveAuthorFromBlog(Int64 blogId, Int64 authorId);

        Task<Blog> SaveOrUpdate(Blog blog);
    }
}
