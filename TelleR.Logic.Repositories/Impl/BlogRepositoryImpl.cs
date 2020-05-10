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
    public class BlogRepositoryImpl : RepositoryBase<Blog, AppDbContext>, IBlogRepository
    {
        public BlogRepositoryImpl(UnitOfWorkBase<AppDbContext> uow) : base(uow) {}

        public async Task<IEnumerable<Blog>> GetAll()
        {
            var blogs = await DbSet.ToArrayAsync();
            return blogs;
        }

        public ValueTask<Blog> GetById(Int64 blogId)
        {
            return DbSet.FindAsync(blogId);
        }

        public Task<Blog> GetByIdWithOwner(Int64 blogId)
        {
            return DbSet.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == blogId);
        }

        public Task<Blog> GetByName(String blogName)
        {
            return DbSet.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Title == blogName);
        }

        public async Task<IEnumerable<Blog>> GetAllByOwner(Int64 userId)
        {
            var blogs = await DbSet.Include(x => x.Owner).Where(x => x.Owner.Id == userId).ToArrayAsync();
            return blogs;
        }
    }
}
