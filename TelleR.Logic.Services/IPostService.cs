using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;

namespace TelleR.Logic.Services
{
    public interface IPostService
    {
        Task<PostResponseDto> getPostById(Int64 postId);

        Task<PostResponseDto> getPostByBlogAndId(Int64 blogId, Int64 postId);

        Task<IEnumerable<PostResponseDto>> getPostsByBlog(Int64 blogId);

        Task<PostResponseDto> createNew(String title, String postContent, String description, Boolean isPublished, Int64 authorId, Int64 blogId);
    }
}
