using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleRPlatformApi.Models;

namespace TelleRPlatformApi.Repositories
{
    public interface IUserRepository
    {
        public User GetByUsername(String username);

        public IEnumerable<User> GetAll();
    }
}
