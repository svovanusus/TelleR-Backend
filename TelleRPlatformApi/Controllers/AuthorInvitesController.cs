using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Data.Exceptions;
using TelleR.Logic.Services;

namespace TelleRPlatformApi.Controllers
{
    [Route("authorInvites")]
    [ApiController]
    public class AuthorInvitesController : ControllerBase
    {
        #region constructors

        public AuthorInvitesController(IAuthorInviteService authorInviteService, IBlogService blogService, IUserService userService)
        {
            _authorInviteService = authorInviteService;
            _blogService = blogService;
            _userService = userService;
        }

        #endregion

        #region public methods

        [HttpGet]
        [Authorize]
        public async Task<AuthorIntiteNotificationsResponseDto> GetNewInviteNotifications()
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId))
            {
                try
                {
                    var meAsReceiverInvites = await _authorInviteService.GetAuthorInvitesForReceiver(userId);
                    var meAsSenderInvites = await _authorInviteService.GetAuthorInviteNotificationForSender(userId);

                    Response.StatusCode = StatusCodes.Status200OK;
                    return new AuthorIntiteNotificationsResponseDto
                    {
                        AsReceiver = meAsReceiverInvites,
                        AsSender = meAsSenderInvites
                    };
                }
                catch(NotFoundException ex)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return new AuthorIntiteNotificationsResponseDto
                    {
                        Message = ex.Message
                    };
                }
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return null;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<CreateAuthorInviteResponseDto> CreateNewInvite([FromBody]CreateAuthorInviteRequestDto model)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _blogService.IsBlogAvailableTochangeForUser(model.BlogId, userId))
            {
                try
                {
                    var result = await _authorInviteService.SendAuthorInvite(userId, model.Email, model.BlogId);

                    if (result) Response.StatusCode = StatusCodes.Status200OK;
                    else Response.StatusCode = StatusCodes.Status400BadRequest;

                    return new CreateAuthorInviteResponseDto
                    {
                        Status = result,
                        Message = result ? "Invite successfully sended." : "Failed to send invite"
                    };
                }
                catch(NotFoundException ex)
                {
                    Response.StatusCode = StatusCodes.Status404NotFound;
                    return new CreateAuthorInviteResponseDto
                    {
                        Status = false,
                        Message = ex.Message
                    };
                }
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return new CreateAuthorInviteResponseDto
            {
                Status = false,
                Message = "Resourse or action is not available for you."
            };
        }

        [HttpPut("answer")]
        [Authorize]
        public async Task<Boolean> AnswerToInvite(Int64 inviteId, Boolean isApprove)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _authorInviteService.IsAuthorInviteAvailabeToAnswerForUser(inviteId, userId))
            {
                var result = await _authorInviteService.AnswerToAuthorInvite(inviteId, isApprove);

                if (result) Response.StatusCode = StatusCodes.Status200OK;
                else Response.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return false;
        }

        [HttpPut("close")]
        [Authorize]
        public async Task<Boolean> CloseInvite(Int64 inviteId)
        {
            Int64 userId;
            if (Int64.TryParse(User.FindFirst(ClaimTypes.NameIdentifier).Value, out userId) && await _authorInviteService.IsAuthorInviteAvailabeToCloseForUser(inviteId, userId))
            {
                var result = await _authorInviteService.CloseAuthorInviteNotification(inviteId);

                if (result) Response.StatusCode = StatusCodes.Status200OK;
                else Response.StatusCode = StatusCodes.Status400BadRequest;

                return result;
            }

            Response.StatusCode = StatusCodes.Status403Forbidden;
            return false;
        }

        #endregion

        #region private fields

        private readonly IAuthorInviteService _authorInviteService;
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        #endregion
    }
}
