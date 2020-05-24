using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsername(String username);

        Task<User> GetByEmail(String email);

        Task<User> GetById(Int64 userId);

        Task<IEnumerable<User>> GetAllByIds(IEnumerable<Int64> ids);

        IQueryable<User> GetAllQueryable();

        Task<User> SaveOrUpdate(User model);
    }
}
