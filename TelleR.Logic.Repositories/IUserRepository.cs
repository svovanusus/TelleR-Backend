using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(String username);

        Task<User> GetById(Int64 userId);

        Task<IEnumerable<User>> GetAll();
    }
}
