using System;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    public class OrderItem : EntityBase<Int64>
    {
        /// <summary>
        /// Content, value of the order item.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Grouping order to the same supplier.
        /// </summary>
        public Order Order { get; set; }

        /// <summary>
        /// Who ordered item.
        /// </summary>
        public User User { get; set; }

        public AuditInfo AuditInfo { get; set; }
    }
}
