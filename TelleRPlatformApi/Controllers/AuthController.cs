using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TelleR.Configuration;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Services;
using TelleR.Logic.Services.Models;

namespace TelleRPlatformApi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region constructors

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region public methods

        [HttpPost]
        public async Task<AuthResponseDto> GetToken([FromBody] AuthDto model)
        {
            var user = await _userService.AuthValidate(model);

            if (user == null)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                return null;
            }

            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthConfig.ISSUER,
                audience: AuthConfig.AUDIENCE,
                notBefore: now,
                claims: new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                },
                expires: now.Add(TimeSpan.FromSeconds(AuthConfig.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthResponseDto
            {
                Token = encodedJwt
            };
        }

        [HttpPost("signup")]
        public async Task<SignupResponseDto> Signup([FromBody] SignupRequestDto model)
        {
            var errors = await _userService.ValidateUserData(new UserDataValidateModel
            {
                UserId = null,
                Username = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            }) as List<String>;

            if (model.Password != model.PasswordConfirm) errors.Add("The passwords are don't match");
            if (!Regex.IsMatch(model.Password, "((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%!?,._-]).{6,30})")) errors.Add("Invalid password format.");

            if (errors.Count == 0)
            {
                var isCreated = await _userService.CreateNew(model);

                if (isCreated)
                {
                    Response.StatusCode = StatusCodes.Status201Created;
                }
                else
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    errors.Add("Unknown error. Try later.");
                }

                return new SignupResponseDto
                {
                    Status = isCreated,
                    Messages = errors
                };
            }

            Response.StatusCode = StatusCodes.Status400BadRequest;
            return new SignupResponseDto
            {
                Status = false,
                Messages = errors
            };
        }

        #endregion

        #region private fields

        private readonly IUserService _userService;

        #endregion
    }
}