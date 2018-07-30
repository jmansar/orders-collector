using System;
using OrdersCollector.Core.Models;
using OrdersCollector.DAL;
using OrdersCollector.DAL.EF;

namespace OrdersCollector.Core.Persistence.Repositories
{

    public interface IOrderItemRepository : IRepository<OrderItem, Int64>
    {
    }

    public class OrderItemRepository : Repository<OrderItem, Int64>, IOrderItemRepository
    {
        public OrderItemRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }
    }
}
