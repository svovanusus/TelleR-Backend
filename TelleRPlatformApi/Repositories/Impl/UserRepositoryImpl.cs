using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleRPlatformApi.Models;
using TelleRPlatformApi.Tools.UnitOfWork;

namespace TelleRPlatformApi.Repositories.Impl
{
    public class UserRepositoryImpl : RepositoryBase<User, AppDbContext>, IUserRepository
    {
        public UserRepositoryImpl(UnitOfWork<AppDbContext> uow) : base(uow)
        { }

        public IEnumerable<User> GetAll()
        {
            return DbSet;
        }

        public User GetByUsername(String username)
        {
            return DbSet.FirstOrDefault(x => x.Username == username);
        }
    }
}
