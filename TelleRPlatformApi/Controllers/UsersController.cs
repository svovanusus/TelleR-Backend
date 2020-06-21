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
using TelleR.Logic.Services;
using TelleR.Logic.Services.Models;

namespace TelleRPlatformApi.Controllers
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region constructors

        public UsersController(IAwsService awsService, IUserService userService)
        {
            _awsService = awsService;
            _userService = userService;
        }

        #endregion

        #region public methods

        [HttpGet]
        [Authorize(Roles = "SuperUser")]
        public async Task<IEnumerable<UserResponseDto>> GetAll()
        {
            return await _userService.GetAll();
        }

        [HttpGet("user")]
        [Authorize]
        public async Task<UserResponseDto> Get(Int64 userId)
        {
            var user = await _userService.Get(userId);
            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                return null;
            }
            return user;
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
            Int64 userId;
            if (!Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return null;
            }

            var user = await _userService.GetProfile(userId);
            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status403Forbidden;
                return null;
            }

            return user;
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

        [HttpPost("uploadAvatar")]
        [Authorize]
        public async Task<FileUploadResposeDto> UploadFile()
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                foreach (var file in Request.Form.Files)
                {
                    var filePath = await _awsService.SaveFileToAws(new FileDto
                    {
                        ContentType = file.ContentType,
                        FileName = file.FileName,
                        ReadStream = file.OpenReadStream()
                    });

                    if (!await _userService.UpdateAvatar(userId, filePath))
                    {
                        Response.StatusCode = StatusCodes.Status400BadRequest;
                        return null;
                    }

                    Response.StatusCode = StatusCodes.Status200OK;
                    return new FileUploadResposeDto
                    {
                        FilePath = filePath
                    };
                }
            }

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return null;
        }

        #endregion

        #region private fields

        private readonly IAwsService _awsService;
        private readonly IUserService _userService;

        #endregion
    }
}