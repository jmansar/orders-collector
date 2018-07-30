using System;
using OrdersCollector.Core.Models;
using OrdersCollector.DAL;
using OrdersCollector.DAL.EF;

namespace OrdersCollector.Core.Persistence.Repositories
{
    public interface IIncrementalUpdateRepository : IRepository<IncrementalUpdate, Int64>
    {
    }

    public class IncrementalUpdateRepository : Repository<IncrementalUpdate, Int64>, IIncrementalUpdateRepository
    {
        public IncrementalUpdateRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }
    }
}
