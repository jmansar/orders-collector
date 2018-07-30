using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrdersCollector.Core.Models;

namespace OrdersCollector.Core.Persistence.Mappings
{
    internal sealed class IncrementalUpdateMap
    {
        private readonly EntityTypeBuilder<IncrementalUpdate> entityBuilder;

        public IncrementalUpdateMap(EntityTypeBuilder<IncrementalUpdate> entityBuilder)
        {
            this.entityBuilder = entityBuilder;
        }

        public void Configure()
        {
            entityBuilder.ToTable("IncrementalUpdates");
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.Property(x => x.Name).IsRequired();
        }
    }
}
