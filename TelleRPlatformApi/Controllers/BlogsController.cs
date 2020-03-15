using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Services;

namespace TelleRPlatformApi.Controllers
{
    [Route("blogs")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
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
    }
}