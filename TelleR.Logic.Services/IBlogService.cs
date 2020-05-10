using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;

namespace TelleR.Logic.Services
{
    public interface IBlogService
    {
        Task<BlogResponseDto> GetById(Int64 blogId);

        Task<BlogResponseDto> GetByName(String blogName);

        Task<IEnumerable<BlogResponseDto>> GetAll(Int64 userId);
    }
}
