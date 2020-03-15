using Microsoft.EntityFrameworkCore;
using Unity;

namespace TelleR.Logic.UnitOfWork.Impl
{
    public class UnitOfWorkFactoryImpl<TContext> : IUnitOfWorkFactory
        where TContext : DbContext
    {
        public UnitOfWorkFactoryImpl(TContext context, IUnityContainer container)
        {
            _context = context;
            _container = container;
        }

        public IUnitOfWork CreateBasicUnitOfWork()
        {
            return new BasicUnitOfWork<TContext>(_context, _container);
        }

        public IUnitOfWork CreateReadonlyUnitOfWork()
        {
            return new ReadonlyUnitOfWork<TContext>(_context, _container);
        }

        private readonly TContext _context;
        private readonly IUnityContainer _container;
    }
}
