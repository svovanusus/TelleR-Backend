using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TelleR.Data.Dto;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Services;

namespace TelleRPlatformApi.Controllers
{
    [Route("files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        #region constructors

        public FilesController(IAwsService awsService)
        {
            _awsService = awsService;
        }

        #endregion

        #region public methods

        [HttpPost("upload")]
        [Authorize]
        public async Task<FileUploadResposeDto> UploadFile()
        {
            foreach (var file in Request.Form.Files)
            {
                var result = await _awsService.SaveFileToAws(new FileDto
                {
                    ContentType = file.ContentType,
                    FileName = file.FileName,
                    ReadStream = file.OpenReadStream()
                });

                return new FileUploadResposeDto
                {
                    FilePath = result
                };
            }

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return null;
        }

        #endregion

        #region private fields

        private readonly IAwsService _awsService;

        #endregion
    }
}