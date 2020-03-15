using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IBlogRepository
    {
        ValueTask<Blog> GetById(Int64 blogId);

        Task<Blog> GetByIdWithOwner(Int64 blogID);

        Task<Blog> GetByName(String blogName);

        Task<IEnumerable<Blog>> GetAll();
    }
}
