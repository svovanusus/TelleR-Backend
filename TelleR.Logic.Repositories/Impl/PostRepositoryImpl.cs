using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Contexts;
using TelleR.Data.Entities;
using TelleR.Logic.UnitOfWork;

namespace TelleR.Logic.Repositories.Impl
{
    public class PostRepositoryImpl : RepositoryBase<Post, AppDbContext>, IPostRepository
    {
        #region constructors

        public PostRepositoryImpl(UnitOfWorkBase<AppDbContext> uow) : base(uow) { }

        #endregion

        #region public methods

        public IQueryable<Post> GetAllForBlogQueryable(long blogId)
        {
            return DbSet.Where(x => x.Blog.Id == blogId);
        }

        public async Task<Post> SaveOrUpdate(Post post)
        {
            var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == post.Id);

            if (entity == null)
            {
                // New

                post.CreateDate = post.UpdateDate = DateTime.Now;
                if (post.IsPublished) post.PublishDate = DateTime.Now;
                var saved = await DbSet.AddAsync(post);
                return saved.Entity;
            }
            else
            {
                // Update

                entity.Title = post.Title;
                entity.PostContent = post.PostContent;
                entity.Description = post.Description;
                entity.IsPublished = post.IsPublished;
                entity.UpdateDate = DateTime.Now;
                if (entity.PublishDate == null && post.IsPublished) entity.PublishDate = DateTime.Now;

                return entity;
            }
        }

        #endregion
    }
}
