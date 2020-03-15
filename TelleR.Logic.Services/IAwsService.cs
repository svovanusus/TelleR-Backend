using System;
using System.Threading.Tasks;
using TelleR.Data.Dto;

namespace TelleR.Logic.Services
{
    public interface IAwsService
    {
        Task<String> SaveFileToAws(FileDto file);
    }
}
