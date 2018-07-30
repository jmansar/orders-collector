using System;
using System.Collections.Generic;
using OrdersCollector.DAL;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Factories
{

    public interface IOrderFactory : IFactory<Order>
    {
        Order Create(Supplier supplier);
    }

    public class OrderFactory : FactoryBase<Order>, IOrderFactory
    {
        public virtual Order Create(Supplier supplier)
        {
            var order = this.Create();
            order.Supplier = supplier;

            return order;
        }

        public override Order Create()
        {
            var order = base.Create();

            order.Items = new List<OrderItem>();
            order.OrderDate = DateTime.Now.Date;

            return order;
        }
    }
}
