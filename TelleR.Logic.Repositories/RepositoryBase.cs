using Microsoft.EntityFrameworkCore;
using TelleR.Logic.UnitOfWork;

namespace TelleR.Logic.Repositories
{
    public abstract class RepositoryBase<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        public UnitOfWorkBase<TContext> UnitOfWork { get; private set; }

        public virtual DbSet<TEntity> DbSet
        {
            get
            {
                return UnitOfWork.Context.Set<TEntity>();
            }
        }

        public RepositoryBase(UnitOfWorkBase<TContext> uow)
        {
            UnitOfWork = uow;
        }
    }
}
