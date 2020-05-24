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
using TelleR.Logic.Services.Models;

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

        // GET /blogs/getInfo
        [HttpGet("getInfo")]
        public async Task<BlogInfoResponseDto> GetBlogInfo(Int64 blogId)
        {
            var blog = await _blogService.GetInfoById(blogId);

            if (blog == null) Response.StatusCode = StatusCodes.Status404NotFound;
            else Response.StatusCode = StatusCodes.Status200OK;

            return blog;
        }

        // GET /blogs/getBlogName
        [HttpGet("getBlogName")]
        public async Task<BlogNameResponseDto> GetBlogName(Int64 blogId)
        {
            var blogName = await _blogService.GetBlogName(blogId);

            if (blogName != null)
            {
                Response.StatusCode = StatusCodes.Status200OK;
                return new BlogNameResponseDto
                {
                    BlogId = blogId,
                    BlogName = blogName
                };
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

        // GET /blogs/isAvailable
        [HttpGet("isAvailable")]
        [Authorize]
        public async Task<Boolean> IsAvailableForUser(Int64 blogId)
        {
            Response.StatusCode = StatusCodes.Status200OK;

            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                return await _blogService.IsBlogAvailableTochangeForUser(blogId, userId);
            }


            return false;
        }

        // GET /blogs/authors
        [HttpGet("authors")]
        [Authorize]
        public async Task<IEnumerable<UserResponseDto>> GetBlogAuthors(Int64 blogId)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _blogService.IsBlogAvailableTochangeForUser(blogId, userId))
            {
                var authors = await _blogService.GetAuthors(blogId);

                if (authors == null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return null;
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return authors;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // POST /blogs/create
        [HttpPost("create")]
        [Authorize]
        public async Task<CreateBlogResponseDto> CreateNewBlog([FromBody] CreateBlogRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                var errors = await _blogService.ValidateBlogInfo(new BlogInfoValidateModel
                {
                    Name = model.Name,
                    Title = model.Title,
                    Description = model.Description,
                }) as List<String>;

                if (errors.Count() > 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new CreateBlogResponseDto
                    {
                        IsError = true,
                        Messages = errors
                    };
                }

                try
                {
                    var blog = await _blogService.CreateNew(model.Name, model.Title, model.Description, model.IsPublic, model.Type, userId);

                    if (blog != null)
                    {
                        Response.StatusCode = StatusCodes.Status201Created;
                        return new CreateBlogResponseDto
                        {
                            IsError = false
                        };
                    }
                }
                catch (Exception)
                {
                    errors.Add("Unknown error");

                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new CreateBlogResponseDto
                    {
                        IsError = true,
                        Messages = errors
                    };
                }
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        //PUT /blogs/update
        [HttpPut("update")]
        [Authorize]
        public async Task<CreateBlogResponseDto> UpdateBlog([FromBody] CreateBlogRequestDto model)
        {
            if (!model.BlogId.HasValue)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return new CreateBlogResponseDto
                {
                    IsError = true,
                    Messages = new List<String>() { "Invalid blogId" }
                };
            }

            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _blogService.IsBlogAvailableTochangeForUser(model.BlogId.Value, userId))
            {
                var errors = await _blogService.ValidateBlogInfo(new BlogInfoValidateModel
                {
                    BlogId = model.BlogId,
                    Name = model.Name,
                    Title = model.Title,
                    Description = model.Description,
                }) as List<String>;

                if (errors.Count() > 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new CreateBlogResponseDto
                    {
                        IsError = true,
                        Messages = errors
                    };
                }

                try
                {
                    var blog = await _blogService.Update(model.BlogId.Value, model.Name, model.Title, model.Description, model.IsPublic, model.Type);

                    if (blog == null)
                    {
                        errors.Add("Blog not found");

                        Response.StatusCode = StatusCodes.Status404NotFound;
                        return new CreateBlogResponseDto
                        {
                            IsError = true,
                            Messages = errors
                        };
                    }

                    Response.StatusCode = StatusCodes.Status200OK;
                    return new CreateBlogResponseDto
                    {
                        IsError = false
                    };
                }
                catch (Exception)
                {
                    errors.Add("Unknown error");

                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new CreateBlogResponseDto
                    {
                        IsError = true,
                        Messages = errors
                    };
                }
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // PUT /blogs/kickAuthor
        [HttpPut("kickAuthor")]
        [Authorize]
        public async Task<Boolean> KickBlogAuthor(Int64 blogId, Int64 authorId)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _blogService.IsBlogAvailableTochangeForUser(blogId, userId))
            {
                var result = await _blogService.KickAuthorFromBlog(blogId, authorId);

                if (result) Response.StatusCode = StatusCodes.Status200OK;
                else Response.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return false;
        }

        #endregion

        #region private fields

        private readonly IBlogService _blogService;

        #endregion
    }
}