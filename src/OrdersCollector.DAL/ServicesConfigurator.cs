using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrdersCollector.DAL.EF;

namespace OrdersCollector.DAL
{
    public sealed class ServicesConfigurator<TDbContext>
        where TDbContext: DbContext
    {
        private readonly IServiceCollection services;
        private readonly Action<DbContextOptionsBuilder> optionsAction;

        public ServicesConfigurator(
            IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction)
        {
            this.services = services;
            this.optionsAction = optionsAction;
        }

        public void Configure()
        {
            services.AddSingleton<IDbHelper, DbHelper>();
            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            services.AddSingleton<IContextProvider, ContextProvider>();
            services.AddDbContext<TDbContext>(optionsAction, ServiceLifetime.Transient, ServiceLifetime.Transient);
            services.AddSingleton<Func<DbContext>>((IServiceProvider serviceProvider) => () => serviceProvider.GetService<TDbContext>());
        }
    }
}
