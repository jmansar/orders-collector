using System;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    public class SupplierAlias : EntityBase<Int64>
    {
        public string Name { get; set; }
        public Supplier Supplier { get; set; }
    }
}
