using System;

namespace TelleR.Logic.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();

        TRepository GetRepository<TRepository>() where TRepository : class;
    }
}
