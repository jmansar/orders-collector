using System.Collections.Generic;
using OrdersCollector.DAL;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Factories
{
    public interface ISupplierFactory : IFactory<Supplier>
    {
    }

    public class SupplierFactory : FactoryBase<Supplier>, ISupplierFactory
    {
        public override Supplier Create()
        {
            var supplier = base.Create();
            supplier.SupplierAliases = new List<SupplierAlias>();
            return supplier;
        }
    }
}
