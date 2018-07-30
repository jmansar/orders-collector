using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class SettingMap
    {
        private readonly EntityTypeBuilder<Setting> entityBuilder;

        public SettingMap(EntityTypeBuilder<Setting> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("Settings");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Key).IsRequired();
            entityBuilder.Property(x => x.Value).IsRequired();
        }
    }
}
