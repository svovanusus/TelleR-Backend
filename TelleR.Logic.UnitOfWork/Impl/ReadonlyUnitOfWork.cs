using Microsoft.EntityFrameworkCore;
using System;
using Unity;

namespace TelleR.Logic.UnitOfWork.Impl
{
    public class ReadonlyUnitOfWork<TContext> : UnitOfWorkBase<TContext>
        where TContext : DbContext
    {
        public ReadonlyUnitOfWork(TContext context, IUnityContainer container)
            : base(context, container) { }

        public override void Commit()
        {
            throw new InvalidOperationException("Nothing to commit in readonly unit of work!");
        }

        public override void Rollback()
        {
            throw new InvalidOperationException("Nothing to rollback in readonly unit of work!");
        }
    }
}
