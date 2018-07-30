using System;
using System.Collections.Generic;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    public class Supplier : EntityBase<Int64>
    {
        public string Name { get; set; }

        public IList<SupplierAlias> SupplierAliases { get; set; }

        public string Phone { get; set; }
    }
}
