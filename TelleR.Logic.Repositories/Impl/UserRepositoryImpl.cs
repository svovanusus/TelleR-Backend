using System;
using System.Collections.Generic;
using System.Linq;
using TelleR.Data.Contexts;
using TelleR.Data.Entities;
using TelleR.Logic.UnitOfWork;

namespace TelleR.Logic.Repositories.Impl
{
    public class UserRepositoryImpl : RepositoryBase<User, AppDbContext>, IUserRepository
    {
        public UserRepositoryImpl(UnitOfWorkBase<AppDbContext> uow) : base(uow) { }

        public IEnumerable<User> GetAll()
        {
            return DbSet;
        }

        public User GetById(long userId)
        {
            return DbSet.FirstOrDefault(x => x.Id == userId);
        }

        public User GetByUsername(String username)
        {
            return DbSet.FirstOrDefault(x => x.Username == username);
        }
    }
}
