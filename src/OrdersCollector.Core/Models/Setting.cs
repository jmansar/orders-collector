using System;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    public class Setting : EntityBase<Int64>
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
