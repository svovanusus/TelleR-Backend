using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Data.Enums;
using TelleR.Logic.Services.Models;

namespace TelleR.Logic.Services
{
    public interface IBlogService
    {
        Task<BlogResponseDto> GetById(Int64 blogId);

        Task<BlogResponseDto> GetByName(String blogName);

        Task<BlogInfoResponseDto> GetInfoById(Int64 blogId);

        Task<String> GetBlogName(Int64 blogId);

        Task<IEnumerable<BlogResponseDto>> GetAll(Int64 userId);

        Task<BlogResponseDto> CreateNew(String name, String title, String desctiption, Boolean isPublic, BlogType type, Int64 ownerId);

        Task<BlogResponseDto> Update(Int64 blogId, String name, String title, String desctiption, Boolean isPublic, BlogType type);

        Task<IEnumerable<String>> ValidateBlogInfo(BlogInfoValidateModel model);

        Task<Boolean> IsBlogAvailableForUser(Int64 blogId, Int64 userId);

        Task<Boolean> IsBlogAvailableTochangeForUser(Int64 blogId, Int64 userId);

        Task<IEnumerable<UserResponseDto>> GetAuthors(Int64 blogId);

        Task<Boolean> KickAuthorFromBlog(Int64 blogId, Int64 authorId);
    }
}
