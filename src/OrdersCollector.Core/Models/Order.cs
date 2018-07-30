using System;
using System.Collections.Generic;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    public class Order : EntityBase<Int64>
    {
        public Supplier Supplier { get; set; }

        public IList<OrderItem> Items { get; set; }

        public DateTime OrderDate { get; set; }

        public User Operator { get; set; }
    }
}
