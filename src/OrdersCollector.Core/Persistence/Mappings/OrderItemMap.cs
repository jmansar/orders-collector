using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class OrderItemMap
    {
        private readonly EntityTypeBuilder<OrderItem> entityBuilder;

        public OrderItemMap(EntityTypeBuilder<OrderItem> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("OrderItems");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Content).IsRequired();
            entityBuilder.HasOne(x => x.User).WithMany().HasForeignKey("User_Id");
            entityBuilder.OwnsOne(x => x.AuditInfo);
        }
    }
}
