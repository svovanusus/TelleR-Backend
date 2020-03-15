using TelleR.Data.Contexts;
using TelleR.Logic.UnitOfWork.Impl;
using Unity;

namespace TelleR.Logic.Tools.Impl
{
    public class TellerDatabaseUnitOfWorkFactoryImpl: UnitOfWorkFactoryImpl<AppDbContext>, ITellerDatabaseUnitOfWorkFactory
    {
        public TellerDatabaseUnitOfWorkFactoryImpl(AppDbContext context, IUnityContainer container) : base(context, container) { }
    }
}
