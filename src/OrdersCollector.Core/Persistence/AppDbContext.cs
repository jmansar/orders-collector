using OrdersCollector.Core.Models;
using Microsoft.EntityFrameworkCore;
using OrdersCollector.Core.Persistence.Mappings;

namespace OrdersCollector.Core.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        internal DbSet<Setting> Settings { get; set; }
        internal DbSet<Order> Orders { get; set; }
        internal DbSet<User> Users { get; set; }
        internal DbSet<Supplier> Suppliers { get; set; }
        internal DbSet<IncrementalUpdate> IncrementalUpdates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new OrderMap(modelBuilder.Entity<Order>()).Configure();
            new OrderItemMap(modelBuilder.Entity<OrderItem>()).Configure();
            new IncrementalUpdateMap(modelBuilder.Entity<IncrementalUpdate>()).Configure();
            new SettingMap(modelBuilder.Entity<Setting>()).Configure();
            new SupplierMap(modelBuilder.Entity<Supplier>()).Configure();
            new SupplierAliasMap(modelBuilder.Entity<SupplierAlias>()).Configure();
            new UserMap(modelBuilder.Entity<User>()).Configure();
        }
    }
}
