using System;
using System.Threading.Tasks;
using TelleR.Data.Dto.Response;

namespace TelleR.Logic.Services
{
    public interface IBlogService
    {
        Task<BlogResponseDto> GetById(Int64 blogId);

        Task<BlogResponseDto> GetByName(String blogName);
    }
}
