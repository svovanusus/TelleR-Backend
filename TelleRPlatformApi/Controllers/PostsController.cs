using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public PostsController(IPostService postService, IBlogService blogService)
        {
            _postService = postService;
            _blogService = blogService;
        }

        #endregion

        #region public methods

        // GET /posts/getPublishedForBlog
        [HttpGet("getPublishedForBlog")]
        [Authorize]
        public async Task<IEnumerable<PostResponseDto>> GetPublishedPostsForBlog(Int64 blogId)
        {
            return await _postService.GetPublishedPostsByBlog(blogId);
        }

        // GET /posts/getAllForBlog
        [HttpGet("getAllForBlog")]
        [Authorize]
        public async Task<IEnumerable<PostResponseDto>> GetAllPostsForBlog(Int64 blogId)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _blogService.IsBlogAvailableForUser(blogId, userId))
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return await _postService.GetPostsByBlog(blogId);
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        [HttpGet("getForEdit")]
        [Authorize]
        public async Task<PostForEditResponseDto> GetForEdit(Int64 postId)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _postService.IsPostAvailableForUser(postId, userId))
            {
                var post = await _postService.GetPostInfoForEdit(postId);
                if (post == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return null;
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return post;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // POST /posts/create
        [HttpPost("create")]
        [Authorize]
        public async Task<PostResponseDto> CreateNewPost([FromBody]CreatePostRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _blogService.IsBlogAvailableForUser(model.BlogId, userId))
            {
                try
                {
                    var description = model.Content.Length > 997
                            ? model.Content.Substring(0, 997) + "..."
                            : model.Content;

                    Response.StatusCode = StatusCodes.Status201Created;
                    return await _postService.CreateNew(model.Title, model.Content, description, model.IsPublished, userId, model.BlogId);
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

        // PUT /posts/update
        [HttpPut("update")]
        [Authorize]
        public async Task<PostResponseDto> Update([FromBody]CreatePostRequestDto model)
        {
            if (!model.PostId.HasValue)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _postService.IsPostAvailableForUser(model.PostId.Value, userId))
            {
                var description = model.Content.Length > 997
                        ? model.Content.Substring(0, 997) + "..."
                        : model.Content;

                var post = await _postService.Update(model.PostId.Value, model.Title, model.Content, description);

                if (post == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return null;
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return post;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // PUT /posts/publish
        [HttpPut("publish")]
        [Authorize]
        public async Task<Boolean> Publish(Int64 postId)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _postService.IsPostAvailableForUser(postId, userId))
            {
                var result = await _postService.Publish(postId);

                if (result) Response.StatusCode = StatusCodes.Status200OK;
                else Response.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return false;
        }

        #endregion

        #region private fields

        private readonly IPostService _postService;
        private readonly IBlogService _blogService;

        #endregion
    }
}