using System;
using System.Collections.Generic;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.DAL;
using OrdersCollector.DAL.EF;

namespace OrdersCollector.Core.Persistence.Repositories
{

    public interface ISupplierRepository : IRepository<Supplier, Int64>
    {
        /// <summary>
        /// Returns supplier with specified name or alias name.
        /// </summary>
        Supplier GetSupplierByName(string name);

        /// <summary>
        /// Return suppliers that with Name like "name%";
        /// </summary>
        /// <param name="name"></param>
        IList<Supplier> GetSuppliersStartWith(string name);

        /// <summary>
        /// Returns all suppliers.
        /// </summary>
        /// <returns></returns>
        IList<Supplier> GetAllSuppliers();
    }

    public class SupplierRepository : Repository<Supplier, Int64>, ISupplierRepository
    {
        public SupplierRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        /// <summary>
        /// Returns supplier with specified name or alias name.
        /// </summary>
        public Supplier GetSupplierByName(string name)
        {
            return this.AsQueryable()
                .Where(s => s.Name.ToLower() == name.ToLower() ||
                    s.SupplierAliases.Any(a => a.Name.ToLower() == name.ToLower())).FirstOrDefault();
        }

        public IList<Supplier> GetSuppliersStartWith(string name)
        {
            return this.AsQueryable()
                .EagerFetch(s => s.SupplierAliases)
                .Where(s => s.Name.ToLower().StartsWith(name.ToLower()) ||
                    s.SupplierAliases.Any(a => a.Name.StartsWith(name.ToLower()))).ToList();
        }

        public IList<Supplier> GetAllSuppliers()
        {
            return this.AsQueryable()
                       .EagerFetch(s => s.SupplierAliases).ToList();
        }
    }
}
