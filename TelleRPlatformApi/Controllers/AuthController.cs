﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TelleRPlatformApi.Dto;
using TelleRPlatformApi.Repositories;
using TelleRPlatformApi.Tools.UnitOfWork;

namespace TelleRPlatformApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UnitOfWork<AppDbContext> _uow;

        public AuthController(UnitOfWork<AppDbContext> uow)
        {
            _uow = uow;
        }

        [HttpPost]
        public Object GetToken([FromBody] AuthDto data)
        {
            var user = _uow.GetRepository<IUserRepository>().GetByUsername(data.username);

            if (user == null || user.Password != data.password)
            {
                Response.StatusCode = 401;
                return new { };
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                   issuer: AuthConfig.ISSUER,
                   audience: AuthConfig.AUDIENCE,
                   notBefore: now,
                   claims: new List<Claim>
                   {
                       new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                       new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
                   },
                   expires: now.Add(TimeSpan.FromSeconds(AuthConfig.LIFETIME)),
                   signingCredentials: new SigningCredentials(AuthConfig.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new
            {
                user = user,
                token = encodedJwt
            };
        }
    }
}