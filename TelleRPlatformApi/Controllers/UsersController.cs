using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TelleR.Data.Dto;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Repositories;
using TelleR.Logic.Services;
using TelleR.Logic.Tools;

namespace TelleRPlatformApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;
        private readonly IAwsService _awsService;

        public UsersController(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory, IAwsService awsService)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
            _awsService = awsService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public async Task<IEnumerable<UserResponseDto>> GetAll()
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                return uow.GetRepository<IUserRepository>().GetAllQueryable().Select(x => new UserResponseDto
                {
                    Id = x.Id,
                    FullName = $"{ x.FirstName } { x.LastName }"
                });
            }
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<UserResponseDto> Get(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = await uow.GetRepository<IUserRepository>().GetById(userId);
                if (user == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return null;
                }

                return new UserResponseDto
                {
                    Id = user.Id,
                    FullName = $"{ user.FirstName } { user.LastName }"
                };
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ProfileResponseDto> GetProfile()
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                String identityfierString = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                Int64 UserId;

                if (identityfierString == null || !Int64.TryParse(identityfierString, out UserId))
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return null;
                }

                var user = await uow.GetRepository<IUserRepository>().GetById(UserId);
                if (user == null)
                {
                    Response.StatusCode = StatusCodes.Status403Forbidden;
                    return null;
                }

                return new ProfileResponseDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }
        }

        [HttpPost("uploadFile")]
        public async Task<FileUploadResposeDto> UploadFile()
        {
            foreach(var file in Request.Form.Files)
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
    }
}