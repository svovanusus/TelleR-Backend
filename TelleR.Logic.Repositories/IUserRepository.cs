using System;
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

        IQueryable<User> GetAllQueryable();

        Task<User> SaveOrUpdate(User model);
    }
}
