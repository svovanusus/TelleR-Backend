using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleR.Data.Contexts;
using TelleR.Data.Entities;
using TelleR.Logic.UnitOfWork;

namespace TelleR.Logic.Repositories.Impl
{
    public class UserRepositoryImpl : RepositoryBase<User, AppDbContext>, IUserRepository
    {
        public UserRepositoryImpl(UnitOfWorkBase<AppDbContext> uow) : base(uow) { }

        public async Task<IEnumerable<User>> GetAll()
        {
            return DbSet;
        }

        public async Task<User> GetById(long userId)
        {
            return DbSet.FirstOrDefault(x => x.Id == userId);
        }

        public async Task<User> GetByUsername(String username)
        {
            return DbSet.FirstOrDefault(x => x.Username == username);
        }
    }
}
