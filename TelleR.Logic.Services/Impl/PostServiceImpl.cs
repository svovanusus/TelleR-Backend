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

        public PostServiceImpl(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory, IBlogService blogService)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
            _blogService = blogService;
        }

        #endregion

        #region public methods

        public async Task<PostResponseDto> GetPostById(Int64 postId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var post = await uow.GetRepository<IPostRepository>().Get(postId);
                if (post == null) return null;

                return new PostResponseDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Author = new UserResponseDto
                    {
                        Id = post.Author.Id,
                        FullName = $"{post.Author.FirstName} {post.Author.LastName}"
                    },
                    CreateDate = post.CreateDate,
                    Content = post.PostContent,
                    Description = post.Description
                };
            }
        }

        public async Task<PostForEditResponseDto> GetPostInfoForEdit(Int64 postId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var post = await uow.GetRepository<IPostRepository>().Get(postId);
                if (post == null) return null;

                return new PostForEditResponseDto
                {
                    PostId = post.Id,
                    Title = post.Title,
                    PostContent = post.PostContent,
                    IsPublished = post.IsPublished,
                    PublishedDate = post.PublishDate
                };
            }
        }

        public async Task<PostResponseDto> GetPostByBlogAndId(Int64 blogId, Int64 postId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var post = await uow.GetRepository<IPostRepository>().Get(postId);
                if (post == null || post.Blog.Id != blogId) return null;

                return new PostResponseDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Author = new UserResponseDto
                    {
                        Id = post.Author.Id,
                        FullName = $"{post.Author.FirstName} {post.Author.LastName}"
                    },
                    CreateDate = post.CreateDate,
                    Content = post.PostContent,
                    Description = post.Description
                };
            }
        }

        public async Task<IEnumerable<PostResponseDto>> GetPostsByBlog(Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var posts = await uow.GetRepository<IPostRepository>().GetAllForBlogQueryable(blogId).Select(x => new PostResponseDto
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
                    CreateDate = x.CreateDate,
                }).ToArrayAsync();
                return posts;
            }
        }

        public async Task<IEnumerable<PostResponseDto>> GetPublishedPostsByBlog(Int64 blogId)
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
                    CreateDate = x.CreateDate,
                }).ToArrayAsync();
                return posts;
            }
        }

        public async Task<Boolean> IsPostAvailableForUser(Int64 postId, Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IPostRepository>().GetPostBlog(postId);
                if (blog == null) return false;

                return await _blogService.IsBlogAvailableForUser(blog.Id, userId);
            }
        }

        public async Task<PostResponseDto> CreateNew(String title, String postContent, String description, Boolean isPublished, Int64 authorId, Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetByIdWithOwner(blogId);

                if (blog == null) throw new NotFoundException();

                var author = blog.Owner;

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
                    CreateDate = savedPost.CreateDate,
                    Author = new UserResponseDto
                    {
                        Id = savedPost.Author.Id,
                        FullName = $"{ savedPost.Author.FirstName } { savedPost.Author.LastName }"
                    }
                };
            }
        }

        public async Task<PostResponseDto> Update(Int64 postId, String title, String postContent, String description)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var postRepo = uow.GetRepository<IPostRepository>();

                var post = await postRepo.Get(postId);
                if (post == null) return null;

                post.Title = title;
                post.PostContent = postContent;
                post.Description = description;

                var savedPost = await uow.GetRepository<IPostRepository>().SaveOrUpdate(post);
                if (savedPost == null) return null;

                uow.Commit();

                return new PostResponseDto
                {
                    Id = savedPost.Id,
                    Title = savedPost.Title,
                    Description = savedPost.Description,
                    Content = savedPost.PostContent,
                    CreateDate = savedPost.CreateDate,
                    Author = new UserResponseDto
                    {
                        Id = savedPost.Author.Id,
                        FullName = $"{ savedPost.Author.FirstName } { savedPost.Author.LastName }"
                    }
                };
            }
        }

        public async Task<Boolean> Publish(Int64 postId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var postRepo = uow.GetRepository<IPostRepository>();

                var post = await postRepo.Get(postId);
                if (post == null) return false;

                post.IsPublished = true;
                var saved = await postRepo.SaveOrUpdate(post);

                uow.Commit();

                return saved != null;
            }
        }

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;
        private readonly IBlogService _blogService;

        #endregion
    }
}
