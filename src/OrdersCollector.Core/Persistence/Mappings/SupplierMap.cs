using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class SupplierMap
    {
        private readonly EntityTypeBuilder<Supplier> entityBuilder;

        public SupplierMap(EntityTypeBuilder<Supplier> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("Suppliers");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.Phone);
            entityBuilder
                .HasMany(x => x.SupplierAliases)
                .WithOne(p => p.Supplier)
                .HasForeignKey("Supplier_Id")
                .IsRequired();
        }
    }
}
