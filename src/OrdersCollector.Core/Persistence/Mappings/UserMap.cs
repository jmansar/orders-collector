using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class UserMap
    {
        private readonly EntityTypeBuilder<User> entityBuilder;

        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("Users");
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Name).IsRequired();
            entityBuilder.Property(x => x.FullName).IsRequired();
            entityBuilder.Property(x => x.IsActive).IsRequired();
            entityBuilder.Property(x => x.IsAdmin).IsRequired();
            entityBuilder.Property(x => x.CanBeOperator).IsRequired();
        }
    }
}
