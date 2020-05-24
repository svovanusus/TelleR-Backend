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
            return DbSet.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Name == blogName);
        }

        public async Task<IEnumerable<Blog>> GetAllByOwner(Int64 userId)
        {
            var blogs = await DbSet.Include(x => x.Owner).Where(x => x.Owner.Id == userId).ToArrayAsync();
            return blogs;
        }

        public async Task<IEnumerable<Blog>> GetAllWithPostsByAuthor(Int64 userId)
        {
            var blogs = await DbSet.Include(x => x.Owner).Include(x => x.Authors).Include(x => x.Posts).Where(x => x.Owner.Id == userId || x.Authors.Any(y => y.AuthorId == userId)).ToArrayAsync();
            return blogs;
        }

        public async Task<IEnumerable<Int64>> GetAuthors (Int64 blogId)
        {
            var blog = await DbSet.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id == blogId);
            return blog.Authors.Select(x => x.AuthorId).ToArray();
        }

        public async Task AddAuthorToBlog(Int64 blogId, Int64 authorId)
        {
            var blog = await DbSet.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id ==  blogId);
            if (blog == null) return;

            if (blog.Authors.Any(x => x.AuthorId == authorId)) return;

            blog.Authors.Add(new BlogAuthor
            {
                BlogId = blog.Id,
                AuthorId = authorId
            });
        }

        public async Task RemoveAuthorFromBlog(Int64 blogId, Int64 authorId)
        {
            var blog = await DbSet.Include(x => x.Authors).FirstOrDefaultAsync(x => x.Id == blogId);
            if (blog == null) return;

            var blogAuthor = blog.Authors.FirstOrDefault(x => x.AuthorId == authorId);
            if (blogAuthor == null) return;

            blog.Authors.Remove(blogAuthor);
        }

        public async Task<Blog> SaveOrUpdate(Blog blog)
        {
            var entity = await DbSet.Include(x => x.Owner).FirstOrDefaultAsync(x => x.Id == blog.Id);

            if (entity == null)
            {
                // new
                blog.CreateDate = DateTime.Now;
                var saved = await DbSet.AddAsync(blog);
                return saved.Entity;
            }
            else
            {
                // update
                entity.Title = blog.Title;
                entity.Description = blog.Description;
                entity.IsPublic = blog.IsPublic;
                entity.Type = blog.Type;
                entity.UpdateDate = DateTime.Now;

                return entity;
            }
        }
    }
}
