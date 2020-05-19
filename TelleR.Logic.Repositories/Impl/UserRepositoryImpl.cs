using Microsoft.EntityFrameworkCore;
using System;
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

        public IQueryable<User> GetAllQueryable()
        {
            return DbSet.AsQueryable();
        }

        public async Task<User> GetById(long userId)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<User> GetByUsername(String username)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetByEmail(String email)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> SaveOrUpdate(User model)
        {
            var entity = await DbSet.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity == null)
            {
                // New
                
                model.CreateDate = DateTime.Now;
                model.UpdateDate = DateTime.Now;
                model.LastActive = DateTime.Now;
                var saved = await DbSet.AddAsync(model);
                return saved.Entity;
            }
            else {
                // Update

                entity.IsActivate = model.IsActivate;
                entity.IsBlocked = model.IsBlocked;
                entity.Username = model.Username;
                entity.Password = model.Password;
                entity.Email = model.Email;
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Role = model.Role;
                entity.LastActive = DateTime.Now;
                entity.UpdateDate = DateTime.Now;

                return entity;
            }
        }
    }
}
