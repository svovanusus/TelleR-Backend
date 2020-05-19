using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Request;
using TelleR.Logic.Services.Models;

namespace TelleR.Logic.Services
{
    public interface IUserService
    {
        Task<IEnumerable<String>> ValidateUserData(UserDataValidateModel model);

        Task<Boolean> CreateNew(SignupRequestDto model);

        Task<AuthModel> AuthValidate(AuthDto model);
    }
}
