using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TelleR.Configuration;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Repositories;
using TelleR.Logic.Tools;

namespace TelleRPlatformApi.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;

        public AuthController(ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
        }

        [HttpPost]
        public AuthResponseDto GetToken([FromBody] AuthDto data)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = uow.GetRepository<IUserRepository>().GetByUsername(data.username);

                if (user == null || user.Password != data.password)
                {
                    Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return null;
                }

                var now = DateTime.UtcNow;

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
        }
    }
}