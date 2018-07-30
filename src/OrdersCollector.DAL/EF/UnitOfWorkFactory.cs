using System;
using Microsoft.EntityFrameworkCore;
using OrdersCollector.Utils.DAL.EF;

namespace OrdersCollector.DAL.EF
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Func<DbContext> dbContextCreator;

        public UnitOfWorkFactory(Func<DbContext> dbContextCreator)
        {
            this.dbContextCreator = dbContextCreator;
        }

        public IUnitOfWork Create()
        {
            return new UnitOfWork(dbContextCreator());
        }
    }
}
