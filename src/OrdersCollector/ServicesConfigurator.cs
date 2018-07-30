using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersCollector.Core.Persistence;

namespace OrdersCollector
{
    public sealed class ServicesConfigurator
    {
        private readonly IConfiguration configuration;
        private readonly IServiceCollection services;

        public ServicesConfigurator(IConfiguration configuration, IServiceCollection services)
        {
            this.configuration = configuration;
            this.services = services;
        }

        public void Configure()
        {
            ConfigureUtilities();
            ConfigureDal();
            ConfigureCore();
        }

        private void ConfigureUtilities()
        {
            new Utils.ServicesConfigurator(services).Configure();
        }

        private void ConfigureDal()
        {
            new DAL.ServicesConfigurator<AppDbContext>(
                services,
                options => options.UseSqlite(configuration.GetConnectionString("AppDb")))
                .Configure();
        }

        private void ConfigureCore()
        {
            new Core.ServicesConfigurator(services)
                .Configure();
        }
    }
}
