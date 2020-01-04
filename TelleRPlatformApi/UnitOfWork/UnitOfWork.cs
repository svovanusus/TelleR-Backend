using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelleRPlatformApi.UnitOfWork
{
    public class UnitOfWork<TContext>
        where TContext : DbContext
    {
        public TContext Context { get; set; }

        private IServiceProvider _provider;

        public UnitOfWork(IServiceProvider provider, TContext context)
        {
            _provider = provider;
            Context = context;
        }

        public TRepository GetRepository<TRepository>()
            where TRepository : class
        {
            return (TRepository)_provider.GetService(typeof(TRepository));
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Rollback()
        { }
    }
}
