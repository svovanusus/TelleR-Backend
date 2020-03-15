using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Repositories;
using TelleR.Logic.Tools;

namespace TelleRPlatformApi.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;

        public PostsController(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
        }

        [HttpGet("getAllForBlog")]
        public PostResponseDto[] GetAllPostsForBlog(Int64 blogId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var posts = uow.GetRepository<IPostRepository>().GetAllForBlog(blogId).Where(x => x.IsPublished).Select(x => new PostResponseDto
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
                }).ToArray();
                return posts;
            }
        }
    }
}