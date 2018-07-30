using Microsoft.Extensions.DependencyInjection;
using OrdersCollector.Utils.Randomization;

namespace OrdersCollector.Utils
{
    public class ServicesConfigurator
    {
        private readonly IServiceCollection services;

        public ServicesConfigurator(IServiceCollection services)
        {
            this.services = services;
        }

        public void Configure()
        {
            services.AddSingleton<IRandomizer, Randomizer>();
        }
    }
}
