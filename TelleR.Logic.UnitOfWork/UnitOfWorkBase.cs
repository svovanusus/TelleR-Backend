using Microsoft.EntityFrameworkCore;
using System;
using Unity;

namespace TelleR.Logic.UnitOfWork
{
    public abstract class UnitOfWorkBase<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        protected UnitOfWorkBase(TContext context, IUnityContainer container)
        {
            _container = container;
            Context = context;
            _container.RegisterInstance(typeof(UnitOfWorkBase<TContext>), this);
            //_container.RegisterInstance(this);

            _disposed = false;
        }

        public TRepository GetRepository<TRepository>()
            where TRepository : class
        {
            if (_disposed)
                throw new ObjectDisposedException("UnitOfWork");

            return _container.Resolve<TRepository>();
        }

        public abstract void Commit();

        public abstract void Rollback();

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                Context.Dispose();

                Context = null;
                _container = null;
            }
        }

        public TContext Context { get; set; }

        private IUnityContainer _container;
        private Boolean _disposed;
    }
}
