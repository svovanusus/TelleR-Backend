using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Data.Entities;
using TelleR.Data.Enums;
using TelleR.Data.Exceptions;
using TelleR.Logic.Repositories;
using TelleR.Logic.Tools;

namespace TelleR.Logic.Services.Impl
{
    public class PostServiceImpl : IPostService
    {
        #region constructors

        public PostServiceImpl(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
        }

        #endregion

        #region public methods

        public Task<PostResponseDto> getPostById(Int64 postId)
        {
            throw new NotImplementedException();
        }

        public Task<PostResponseDto> getPostByBlogAndId(Int64 blogId, Int64 postId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PostResponseDto>> getPostsByBlog(Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var posts = await uow.GetRepository<IPostRepository>().GetAllForBlogQueryable(blogId).Where(x => x.IsPublished).Select(x => new PostResponseDto
                {
                    Id = x.Id,
                    Author = new UserResponseDto
                    {
                        Id = x.Author.Id,
                        FullName = $"{x.Author.FirstName} {x.Author.LastName}"
                    },
                    Title = x.Title,
                    Description = x.Description,
                    Content = x.PostContent,
                    CreateDate = x.PublishDate,
                }).ToArrayAsync();
                return posts;
            }
        }

        public async Task<PostResponseDto> createNew(String title, String postContent, String description, Boolean isPublished, Int64 authorId, Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetByIdWithOwner(blogId);

                if (blog == null) throw new NotFoundException();

                var author = blog.Owner;

                if (blog.Type == BlogType.Personal && blog.Owner.Id != authorId) throw new AccessDeniedException();

                if (author.Id != authorId) author = await uow.GetRepository<IUserRepository>().GetById(authorId);

                var post = new Post
                {
                    Title = title,
                    PostContent = postContent,
                    Description = description,
                    IsPublished = isPublished,
                    Blog = blog,
                    Author = author
                };

                var savedPost = await uow.GetRepository<IPostRepository>().SaveOrUpdate(post);
                if (savedPost == null) return null;

                uow.Commit();

                return new PostResponseDto
                {
                    Id = savedPost.Id,
                    Title = savedPost.Title,
                    Description = savedPost.Description,
                    Content = savedPost.PostContent,
                    CreateDate = savedPost.PublishDate,
                    Author = new UserResponseDto
                    {
                        Id = savedPost.Author.Id,
                        FullName = $"{ savedPost.Author.FirstName } { savedPost.Author.LastName }"
                    }
                };
            }
        }

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;

        #endregion
    }
}
