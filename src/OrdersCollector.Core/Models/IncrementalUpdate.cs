using System;
using OrdersCollector.DAL;

namespace OrdersCollector.Core.Models
{
    /// <summary>
    /// Represent incremental update to the DB that was performed.
    /// </summary>
    public class IncrementalUpdate : EntityBase<Int64>
    {
        /// <summary>
        /// Name of the sql script.
        /// </summary>
        public string Name { get; set; }
    }
}
