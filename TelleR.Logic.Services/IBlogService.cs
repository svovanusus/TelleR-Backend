using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;
using TelleR.Data.Enums;

namespace TelleR.Logic.Services
{
    public interface IBlogService
    {
        Task<BlogResponseDto> GetById(Int64 blogId);

        Task<BlogResponseDto> GetByName(String blogName);

        Task<IEnumerable<BlogResponseDto>> GetAll(Int64 userId);

        Task<BlogResponseDto> CreateNew(String name, String title, String desctiption, Boolean isPublic, BlogType type, Int64 ownerId);
    }
}
