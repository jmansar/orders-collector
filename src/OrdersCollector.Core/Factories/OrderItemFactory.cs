using OrdersCollector.Core.Models;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Factories
{
    public interface IOrderItemFactory : IFactory<OrderItem>
    {
        OrderItem Create(Order order);
    }

    public class OrderItemFactory : FactoryBase<OrderItem>, IOrderItemFactory
    {
        public OrderItem Create(Order order)
        {
            var item = this.Create();

            item.Order = order;

            return item;
        }
    }
}
