using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelleR.Data.Dto.Request;
using TelleR.Data.Dto.Response;
using TelleR.Data.Entities;
using TelleR.Data.Enums;
using TelleR.Logic.Repositories;
using TelleR.Logic.Services.Models;
using TelleR.Logic.Tools;

namespace TelleR.Logic.Services.Impl
{
    public class UserServiceImpl : IUserService
    {
        #region constructors

        public UserServiceImpl (ITellerDatabaseUnitOfWorkFactory tellerDatabaseUnitOfWorkFactory)
        {
            _tellerDatabaseUnitOfWorkFactory = tellerDatabaseUnitOfWorkFactory;
        }

        #endregion

        #region public methods

        public async Task<UserResponseDto> Get(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = await uow.GetRepository<IUserRepository>().GetById(userId);
                if (user == null)
                {
                    return null;
                }

                return new UserResponseDto
                {
                    Id = user.Id,
                    FullName = $"{ user.FirstName } { user.LastName }",
                    Avatar = user.Avatar
                };
            }
        }

        public async Task<ProfileResponseDto> GetProfile(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = await uow.GetRepository<IUserRepository>().GetById(userId);
                if (user == null)
                {
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

        public async Task<IEnumerable<UserResponseDto>> GetAll()
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var result = await uow.GetRepository<IUserRepository>().GetAllQueryable().Select(x => new UserResponseDto
                {
                    Id = x.Id,
                    FullName = $"{ x.FirstName } { x.LastName }",
                    Avatar = x.Avatar
                }).ToArrayAsync();

                return result;
            }
        }

        public async Task<UserInfoResponseDto> GetUserInfo(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = await userRepo.GetById(userId);
                if (user == null) return null;

                user.LastActive = DateTime.Now;
                await userRepo.SaveOrUpdate(user);
                uow.Commit();

                return new UserInfoResponseDto
                {
                    UserId = user.Id,
                    Role = user.Role,
                    Avatar = user.Avatar,
                    FullName = $"{user.FirstName} {user.LastName}"
                };
            }
        }

        public async Task<UserInfoForEditResponseDto> GetUserInfoForEdit(Int64 userId)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();
                var user = await userRepo.GetById(userId);
                if (user == null) return null;

                user.LastActive = DateTime.Now;
                await userRepo.SaveOrUpdate(user);
                uow.Commit();

                return new UserInfoForEditResponseDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }
        }

        public async Task<AuthModel> AuthValidate(AuthDto model)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var user = await uow.GetRepository<IUserRepository>().GetByUsername(model.Username.ToLower());
                if (user == null || !CheckPassword(model.Password, user.Password))
                {
                    return null;
                }

                return new AuthModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role
                };
            }
        }

        public async Task<Boolean> CreateNew(SignupRequestDto model)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var user = new User
                {
                    Username = model.Username.Trim().ToLower(),
                    Password = EncryptPassword(model.Password),
                    Email = model.Email.Trim().ToLower(),
                    IsActivate = true,
                    IsBlocked = false,
                    Role = UserRole.User,
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim()
                };

                var savedUser = await uow.GetRepository<IUserRepository>().SaveOrUpdate(user);

                uow.Commit();

                return savedUser != null;
            }
        }

        public async Task<Boolean> Update(Int64 userId, String username, String email, String firstName, String lastName)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = await userRepo.GetById(userId);
                if (user == null) return false;

                user.Username = username;
                user.Email = email;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.LastActive = DateTime.Now;

                var savedUser = await userRepo.SaveOrUpdate(user);

                uow.Commit();

                return savedUser != null;
            }
        }

        public async Task<Boolean> ChangePassword(Int64 userId, String newPassword)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = await userRepo.GetById(userId);
                if (user == null) return false;

                user.Password = EncryptPassword(newPassword);

                var savedUser = await userRepo.SaveOrUpdate(user);

                uow.Commit();

                return savedUser != null;
            }
        }

        public async Task<Boolean> ValidatePasswordForUser(Int64 userId, String password)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = await userRepo.GetById(userId);
                if (user == null) return false;

                return CheckPassword(password, user.Password);
            }
        }

        public async Task<IEnumerable<String>> ValidateUserData(UserDataValidateModel model)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateReadonlyUnitOfWork())
            {
                var messages = new List<String>();

                if (String.IsNullOrEmpty(model.FirstName.Trim())) messages.Add("First name is empty.");

                if (String.IsNullOrEmpty(model.LastName.Trim())) messages.Add("Last name is empty.");

                var userRepo = uow.GetRepository<IUserRepository>();

                if (!Regex.IsMatch(model.Username.Trim(), "^[a-z]([a-z]|[0-9]|_|-)*$")) messages.Add("Username contains unresolved symbols.");
                else
                {
                    var user = await userRepo.GetByUsername(model.Username.Trim().ToLower());
                    if (user != null && user.Id != model.UserId) messages.Add("User with this username already exists.");
                }

                if (!Regex.IsMatch(model.Email.Trim(), ".+.@.+\\..+")) messages.Add("Email has incorrect format.");
                else
                {
                    var user = await userRepo.GetByEmail(model.Email.Trim().ToLower());
                    if (user != null && user.Id != model.UserId) messages.Add("User with this E-Mail already exists.");
                }

                return messages;
            }
        }

        public async Task<Boolean> UpdateAvatar(Int64 userId, String filePath)
        {
            using (var uow = _tellerDatabaseUnitOfWorkFactory.CreateBasicUnitOfWork())
            {
                var userRepo = uow.GetRepository<IUserRepository>();

                var user = await userRepo.GetById(userId);
                if (user == null) return false;

                user.Avatar = filePath;

                var saved = await userRepo.SaveOrUpdate(user);

                uow.Commit();

                return saved != null;
            }
        }

        #endregion

        #region private methods

        private String EncryptPassword(String password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        private Boolean CheckPassword(String password, String hash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region private fields

        private readonly ITellerDatabaseUnitOfWorkFactory _tellerDatabaseUnitOfWorkFactory;

        #endregion
    }
}
