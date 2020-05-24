using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelleR.Data.Dto;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Repositories;
using TelleR.Logic.Services;
using TelleR.Logic.Services.Models;
using TelleR.Logic.Tools;

namespace TelleRPlatformApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region constructors

        public UsersController(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory, IAwsService awsService, IUserService userService)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
            _awsService = awsService;
            _userService = userService;
        }

        #endregion

        #region public methods

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

        [HttpGet("info")]
        [Authorize]
        public async Task<UserInfoResponseDto> GetMyInfo()
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                var user = await _userService.GetUserInfo(userId);

                if (user == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return null;
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return user;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // GET /users/infoForEdit
        [HttpGet("infoForEdit")]
        [Authorize]
        public async Task<UserInfoForEditResponseDto> getMyInfoForEdit()
        {
            Int64 userId;
            if(Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                var user = await _userService.GetUserInfoForEdit(userId);

                if (user == null)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return null;
                }

                Response.StatusCode = StatusCodes.Status200OK;
                return user;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
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

        // PUT /users/updateInfo
        [HttpPut("updateInfo")]
        [Authorize]
        public async Task<SignupResponseDto> UpdateUserInfo([FromBody]UserInfoUpdateRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                var messagses = await _userService.ValidateUserData(new UserDataValidateModel
                {
                    UserId = userId,
                    Username = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                }) as List<String>;

                if (messagses.Count() > 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new SignupResponseDto
                    {
                        Status = false,
                        Messages = messagses
                    };
                }

                var result = await _userService.Update(userId, model.Username, model.Email, model.FirstName, model.LastName);

                if (!result)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    messagses.Add("No data was saved.");
                }
                else Response.StatusCode = StatusCodes.Status200OK;

                return new SignupResponseDto
                {
                    Status = result,
                    Messages = messagses
                };
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        // PUT /users/changePassword
        [HttpPut("changePassword")]
        [Authorize]
        public async Task<SignupResponseDto> UpdateUserPassword([FromBody]UserPasswordUpdateRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                var messages = new List<String>();

                if (!Regex.IsMatch(model.NewPassword, "((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%!?,._-]).{6,30})")) messages.Add("Invalid password format.");
                else if (model.NewPassword != model.PasswordConfirm) messages.Add("The passwords are don't match");
                else if (!await _userService.ValidatePasswordForUser(userId, model.OldPassword)) messages.Add("Old password is invalid.");

                if (messages.Count() > 0)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return new SignupResponseDto
                    {
                        Status = false,
                        Messages = messages
                    };
                }

                var result = await _userService.ChangePassword(userId, model.NewPassword);
                if (!result)
                {
                    messages.Add("New password wasn't be saved.");
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                } else Response.StatusCode = StatusCodes.Status200OK;

                return new SignupResponseDto
                {
                    Status = result,
                    Messages = messages
                };
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
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

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;
        private readonly IAwsService _awsService;
        private readonly IUserService _userService;

        #endregion
    }
}