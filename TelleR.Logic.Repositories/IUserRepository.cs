using System;
using System.Collections.Generic;
using TelleR.Data.Entities;

namespace TelleR.Logic.Repositories
{
    public interface IUserRepository
    {
        User GetByUsername(String username);

        User GetById(Int64 userId);

        IEnumerable<User> GetAll();
    }
}
