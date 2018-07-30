using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class SupplierAliasMap
    {
        private readonly EntityTypeBuilder<SupplierAlias> entityBuilder;

        public SupplierAliasMap(EntityTypeBuilder<SupplierAlias> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("SupplierAliases");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.HasOne(x => x.Supplier);
        }
    }
}
