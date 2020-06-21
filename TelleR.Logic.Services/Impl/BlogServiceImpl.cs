using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Data.Entities;
using TelleR.Data.Enums;
using TelleR.Logic.Repositories;
using TelleR.Logic.Services.Models;
using TelleR.Logic.Tools;

namespace TelleR.Logic.Services.Impl
{
    public class BlogServiceImpl : IBlogService
    {
        #region constructors

        public BlogServiceImpl(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
        }

        #endregion

        #region public methods

        public async Task<BlogResponseDto> GetById(long blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetByIdWithOwner(blogId);

                if (blog == null) return null;

                return new BlogResponseDto
                {
                    Id = blog.Id,
                    Name = blog.Name,
                    Title = blog.Title,
                    Author = new UserResponseDto
                    {
                        Id = blog.Owner.Id,
                        FullName = $"{ blog.Owner.FirstName } { blog.Owner.LastName }",
                        Avatar = blog.Owner.Avatar
                    }
                };
            }
        }

        public async Task<BlogResponseDto> GetByName(string blogName)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetByName(blogName);

                if (blog == null) return null;

                return new BlogResponseDto
                {
                    Id = blog.Id,
                    Name = blog.Name,
                    Title = blog.Title,
                    Author = new UserResponseDto
                    {
                        Id = blog.Owner.Id,
                        FullName = $"{ blog.Owner.FirstName } { blog.Owner.LastName }",
                        Avatar = blog.Owner.Avatar
                    }
                };
            }
        }

        public async Task<BlogInfoResponseDto> GetInfoById(Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetById(blogId);

                if (blog == null) return null;

                return new BlogInfoResponseDto
                {
                    BlogId = blog.Id,
                    Name = blog.Name,
                    Title = blog.Title,
                    Description = blog.Description
                };
            }
        }

        public async Task<String> GetBlogName(Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blog = await uow.GetRepository<IBlogRepository>().GetById(blogId);
                if (blog == null) return null;

                return blog.Name;
            }
        }

        public async Task<IEnumerable<BlogResponseDto>> GetAll(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var blogs = await uow.GetRepository<IBlogRepository>().GetAllWithPostsByAuthor(userId);

                return blogs.Select(x => new BlogResponseDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Author = new UserResponseDto
                    {
                        Id = x.Owner.Id,
                        FullName = $"{ x.Owner.FirstName } { x.Owner.LastName }",
                        Avatar = x.Owner.Avatar
                    },
                    PostsCount = x.Posts.Count()
                });
            }
        }

        public async Task<BlogResponseDto> CreateNew(String name, String title, String description, Boolean isPublic, BlogType type, Int64 ownerId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var owner = await uow.GetRepository<IUserRepository>().GetById(ownerId);

                if (owner == null) return null;

                var blog = new Blog {
                    Name = name.Trim().ToLower(),
                    Title = title.Trim(),
                    Description = description.Trim(),
                    IsPublic = isPublic,
                    Type = type,
                    Owner = owner
                };

                var savedBlog = await uow.GetRepository<IBlogRepository>().SaveOrUpdate(blog);
                uow.Commit();

                return new BlogResponseDto
                {
                    Id = savedBlog.Id,
                    Name = savedBlog.Name,
                    Title = savedBlog.Title,
                    Author = new UserResponseDto
                    {
                        Id = savedBlog.Owner.Id,
                        FullName = $"{ savedBlog.Owner.FirstName } { savedBlog.Owner.LastName }",
                        Avatar = savedBlog.Owner.Avatar
                    }
                };
            }
        }

        public async Task<BlogResponseDto> Update(Int64 blogId, String name, String title, String desctiption, Boolean isPublic, BlogType type)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var blogRepo = uow.GetRepository<IBlogRepository>();

                var blog = await blogRepo.GetById(blogId);
                if (blog == null) return null;

                blog.Name = name;
                blog.Title = title;
                blog.Description = desctiption;
                blog.IsPublic = isPublic;
                blog.Type = type;

                var savedBlog = await uow.GetRepository<IBlogRepository>().SaveOrUpdate(blog);
                uow.Commit();

                return new BlogResponseDto
                {
                    Id = savedBlog.Id,
                    Name = savedBlog.Name,
                    Title = savedBlog.Title,
                    Author = new UserResponseDto
                    {
                        Id = savedBlog.Owner.Id,
                        FullName = $"{ savedBlog.Owner.FirstName } { savedBlog.Owner.LastName }",
                        Avatar = savedBlog.Owner.Avatar
                    }
                };
            }
        }

        public async Task<IEnumerable<String>> ValidateBlogInfo(BlogInfoValidateModel model)
        {
            var messages = new List<String>();

            if (String.IsNullOrEmpty(model.Name.Trim())) messages.Add("Blog name is empty.");
            else if (!Regex.IsMatch(model.Name.Trim().ToLower(), "[a-z][a-z0-9-_]*")) messages.Add("Invalid blog name.");
            else
            {
                using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
                {
                    var blog = await uow.GetRepository<IBlogRepository>().GetByName(model.Name.Trim().ToLower());
                    if (blog != null && blog.Id != model.BlogId) messages.Add("Blog with this name already exists.");
                }
            }

            if (String.IsNullOrEmpty(model.Title.Trim())) messages.Add("Blog title is empty");

            return messages;
        }

        public async Task<Boolean> IsBlogAvailableForUser(Int64 blogId, Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = await uow.GetRepository<IUserRepository>().GetById(userId);
                if (user == null) return false;

                if (user.Role == UserRole.SuperUser) return true;

                var blog = await uow.GetRepository<IBlogRepository>().GetByIdWithOwner(blogId);
                if (blog == null) return false;

                if (blog.Owner.Id == userId) return true;

                var blogAuthors = await uow.GetRepository<IBlogRepository>().GetAuthors(blogId);
                if (blogAuthors == null) return false;

                return blogAuthors.Any(x => x == userId);
            }
        }

        public async Task<Boolean> IsBlogAvailableTochangeForUser(Int64 blogId, Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = await uow.GetRepository<IUserRepository>().GetById(userId);
                if (user == null) return false;

                if (user.Role == UserRole.SuperUser) return true;

                var blog = await uow.GetRepository<IBlogRepository>().GetByIdWithOwner(blogId);

                return blog != null && blog.Owner.Id == userId;
            }
        }

        public async Task<IEnumerable<UserResponseDto>> GetAuthors(Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var authorsIds = await uow.GetRepository<IBlogRepository>().GetAuthors(blogId);
                if (authorsIds == null) return null;

                var authors = await uow.GetRepository<IUserRepository>().GetAllByIds(authorsIds);

                return authors.Select(x => new UserResponseDto
                {
                    Id = x.Id,
                    FullName = $"{ x.FirstName } { x.LastName }",
                    Avatar = x.Avatar
                });
            }
        }

        public async Task<Boolean> KickAuthorFromBlog(Int64 blogId, Int64 authorId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                try
                {
                    await uow.GetRepository<IBlogRepository>().RemoveAuthorFromBlog(blogId, authorId);

                    uow.Commit();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;

        #endregion
    }
}
