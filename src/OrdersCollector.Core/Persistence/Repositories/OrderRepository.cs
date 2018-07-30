using System;
using System.Linq;
using OrdersCollector.DAL;
using OrdersCollector.Core.Models;
using OrdersCollector.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace OrdersCollector.Core.Persistence.Repositories
{

    public interface IOrderRepository : IRepository<Order, Int64>
    {
        /// <summary>
        /// Returns active order by supplier
        /// </summary>
        Order GetActiveOrder(Supplier supplier);
    }

    public class OrderRepository : Repository<Order, Int64>, IOrderRepository
    {
        public OrderRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        /// <summary>
        /// Returns active order by supplier
        /// </summary>
        public Order GetActiveOrder(Supplier supplier)
        {
            var order = this.AsQueryable()
                .Include(a => a.Supplier)
                .Include(a => a.Items)
                .ThenInclude(a => a.User)
                .Where(a => a.Supplier.Id == supplier.Id)
                .Active()
                .FirstOrDefault();

            return order;
        }
    }

    public static class OrderQueryExtensions
    {
        /// <summary>
        /// Filters for active orders.
        /// </summary>
        public static IQueryable<Order> Active(this IQueryable<Order> query)
        {
            var today = DateTime.Now.Date;
            query = query.Where(o => o.OrderDate == today);
            return query;
        }
    }
}
