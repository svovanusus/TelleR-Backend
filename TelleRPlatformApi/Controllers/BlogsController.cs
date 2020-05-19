using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Data.Enums;
using TelleR.Logic.Services;

namespace TelleRPlatformApi.Controllers
{
    [Route("blogs")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        #region constructors

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #endregion

        #region public methods

        // GET /blogs
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<BlogResponseDto>> GetAll()
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value , out userId))
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return await _blogService.GetAll(userId);
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // GET /blogs/byId
        [HttpGet("byId")]
        public async Task<BlogResponseDto> GetById(Int64 blogId)
        {
            var blog = await _blogService.GetById(blogId);
            
            if (blog != null)
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return blog;
            }

            Response.StatusCode = StatusCodes.Status404NotFound;
            return null;
        }

        // GET /blogs/byName
        [HttpGet("byName")]
        public async Task<BlogResponseDto> GetByName(String blogName)
        {
            var blog = await _blogService.GetByName(blogName);

            if (blog != null)
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return blog;
            }

            Response.StatusCode = StatusCodes.Status404NotFound;
            return null;
        }

        // GET /blogs/types
        [HttpGet("types")]
        public IEnumerable<DictionaryItemDto<BlogType>> getBlogTypes()
        {
            var type = typeof(BlogType);

            var dictionary = new List<DictionaryItemDto<BlogType>>();

            foreach (var item in Enum.GetValues(type))
            {
                var member = type.GetMember(item.ToString())[0];
                var description = member.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
                dictionary.Add(new DictionaryItemDto<BlogType>
                {
                    label = description?.Description ?? item.ToString(),
                    value = (BlogType)item
                });
            }

            return dictionary;
        }

        // POST /blogs/create
        [HttpPost("create")]
        [Authorize]
        public async Task<BlogResponseDto> CreateNewBlog([FromBody] CreateBlogRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                var blog = await _blogService.CreateNew(model.Name, model.Title, model.Description, model.IsPublic, model.Type, userId);
                
                if (blog != null)
                {
                    Response.StatusCode = StatusCodes.Status201Created;
                    return blog;
                }

                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        #endregion

        #region private fields

        private readonly IBlogService _blogService;

        #endregion
    }
}