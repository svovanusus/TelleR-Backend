using Microsoft.EntityFrameworkCore;
using Unity;

namespace TelleR.Logic.UnitOfWork.Impl
{
    public class BasicUnitOfWork<TContext> : UnitOfWorkBase<TContext>
        where TContext : DbContext
    {
        public BasicUnitOfWork(TContext context, IUnityContainer container)
            : base(context, container) { }

        public override void Commit()
        {
            Context.SaveChanges();
        }

        public override void Rollback() { }
    }
}
