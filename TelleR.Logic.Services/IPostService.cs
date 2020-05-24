using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;

namespace TelleR.Logic.Services
{
    public interface IPostService
    {
        Task<PostResponseDto> GetPostById(Int64 postId);

        Task<PostForEditResponseDto> GetPostInfoForEdit(Int64 postId);

        Task<PostResponseDto> GetPostByBlogAndId(Int64 blogId, Int64 postId);

        Task<IEnumerable<PostResponseDto>> GetPostsByBlog(Int64 blogId);

        Task<IEnumerable<PostResponseDto>> GetPublishedPostsByBlog(Int64 blogId);

        Task<Boolean> IsPostAvailableForUser(Int64 postId, Int64 userId);

        Task<PostResponseDto> CreateNew(String title, String postContent, String description, Boolean isPublished, Int64 authorId, Int64 blogId);

        Task<PostResponseDto> Update(Int64 postId, String title, String postContent, String description);

        Task<Boolean> Publish(Int64 postId);
    }
}
