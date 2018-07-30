using System;
using System.Collections.Generic;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.DAL;
using OrdersCollector.DAL.EF;

namespace OrdersCollector.Core.Persistence.Repositories
{
    public interface IUserRepository : IRepository<User, Int64>
    {
        /// <summary>
        /// Return list of users that belong to the operators pool for the order.
        /// </summary>
        IList<User> GetUserOperatorPool(Order order);

        /// <summary>
        /// Get by user name.
        /// </summary>
        User GetByName(string name);
    }

    public class UserRepository : Repository<User, Int64>, IUserRepository
    {
        // TODO: Persistence layer rework - remove this reference to order repository
        private readonly IOrderRepository orderRepository;

        public UserRepository(IContextProvider contextProvider, IOrderRepository orderRepository) : base(contextProvider)
        {
            this.orderRepository = orderRepository;
        }

        /// <summary>
        /// Return list of users that belong to the operators pool for the order.
        /// </summary>
        public IList<User> GetUserOperatorPool(Order order)
        {
            return orderRepository.AsQueryable()
                .Where(o => o.Id == order.Id)
                .SelectMany(o => o.Items)
                .Select(o => o.User)
                .Where(u => u.CanBeOperator)
                .Distinct()
                .ToList();
        }

        /// <summary>
        /// Get by user name.
        /// </summary>
        /// <param name="name">The name.</param>
        public User GetByName(string name)
        {
            return this.AsQueryable()
                       .Where(u => u.Name == name)
                       .FirstOrDefault();
        }
    }
}
