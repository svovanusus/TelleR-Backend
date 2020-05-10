using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelleR.Data.Dto.Response;
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

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<BlogResponseDto>> GetAll()
        {
            var userId = Int64.MinValue;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value , out userId))
            {
                return await _blogService.GetAll(userId);
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        [HttpGet("byId")]
        public async Task<BlogResponseDto> GetById(Int64 blogId)
        {
            return await _blogService.GetById(blogId);
        }

        [HttpGet("byName")]
        public async Task<BlogResponseDto> GetByName(String blogName)
        {
            return await _blogService.GetByName(blogName);
        }

        #endregion

        #region private fields

        private readonly IBlogService _blogService;

        #endregion
    }
}