using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class OrderMap
    {
        private readonly EntityTypeBuilder<Order> entityBuilder;

        public OrderMap(EntityTypeBuilder<Order> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("Orders");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.OrderDate).IsRequired();
            entityBuilder.HasOne(x => x.Supplier).WithMany().HasForeignKey("Supplier_Id");
            entityBuilder.HasOne(x => x.Operator).WithMany().HasForeignKey("Operator_Id");
            entityBuilder
                .HasMany(x => x.Items)
                .WithOne(p => p.Order)
                .HasForeignKey("Order_Id")
                .IsRequired();
        }
    }
}
