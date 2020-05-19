using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Data.Exceptions;
using TelleR.Logic.Services;

namespace TelleRPlatformApi.Controllers
{
    [Route("posts")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        #region constructors

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        #endregion

        #region public methods

        // GET /posts/getAllForBlog
        [HttpGet("getAllForBlog")]
        public async Task<IEnumerable<PostResponseDto>> GetAllPostsForBlog(Int64 blogId)
        {
            return await _postService.getPostsByBlog(blogId);
        }

        // POST /posts/create
        [HttpPost("create")]
        public async Task<PostResponseDto> CreateNewPost([FromBody]CreatePostRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                try
                {
                    var description = "";

                    Response.StatusCode = StatusCodes.Status201Created;
                    return await _postService.createNew(model.Title, model.Content, description, model.IsPublished, userId, model.BlogId);
                }
                catch (NotFoundException)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return null;
                }
                catch (AccessDeniedException)
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        #endregion

        #region private fields

        private readonly IPostService _postService;

        #endregion
    }
}