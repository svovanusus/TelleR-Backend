using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Logic.Services.Models;

namespace TelleR.Logic.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> Get(Int64 userId);

        Task<ProfileResponseDto> GetProfile(Int64 userId);

        Task<IEnumerable<UserResponseDto>> GetAll();

        Task<UserInfoResponseDto> GetUserInfo(Int64 userId);

        Task<UserInfoForEditResponseDto> GetUserInfoForEdit(Int64 userId);

        Task<IEnumerable<String>> ValidateUserData(UserDataValidateModel model);

        Task<Boolean> CreateNew(SignupRequestDto model);

        Task<Boolean> Update(Int64 userId, String username, String email, String firstName, String lastName);

        Task<Boolean> ChangePassword(Int64 userId, String newPassword);

        Task<Boolean> ValidatePasswordForUser(Int64 userId, String password);

        Task<AuthModel> AuthValidate(AuthDto model);

        Task<Boolean> UpdateAvatar(Int64 userId, String filePath);
    }
}
