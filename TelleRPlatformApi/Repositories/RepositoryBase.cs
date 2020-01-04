using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelleRPlatformApi.Tools.UnitOfWork;

namespace TelleRPlatformApi.Repositories
{
    public abstract class RepositoryBase<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        public UnitOfWork<TContext> _uow { get; private set; }

        public virtual DbSet<TEntity> DbSet
        {
            get
            {
                return _uow.Context.Set<TEntity>();
            }
        }

        public RepositoryBase(UnitOfWork<TContext> uow)
        {
            _uow = uow;
        }
    }
}
