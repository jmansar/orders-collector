using System;
using Microsoft.Extensions.DependencyInjection;
using OrdersCollector.Core.Commands;
using OrdersCollector.Core.Factories;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Core.Setup;

namespace OrdersCollector.Core
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
            ConfigureRepositories();
            ConfigureCommands();
            ConfigureFactories();
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            services.AddSingleton<IDbUpdater, DbUpdater>();
        }

        private void ConfigureFactories()
        {
            services.AddSingleton<IOrderFactory, OrderFactory>();
            services.AddSingleton<IOrderItemFactory, OrderItemFactory>();
            services.AddSingleton<ISupplierFactory, SupplierFactory>();
            services.AddSingleton<IUserFactory, UserFactory>();
        }

        private void ConfigureRepositories()
        {
            services.AddSingleton<IIncrementalUpdateRepository, IncrementalUpdateRepository>();
            services.AddSingleton<IOrderItemRepository, OrderItemRepository>();
            services.AddSingleton<IOrderRepository, OrderRepository>();
            services.AddSingleton<ISupplierRepository, SupplierRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
        }

        private void ConfigureCommands()
        {
            services.AddSingleton<ICommandFactory, CommandFactory>();
            services.AddSingleton<ICommandEventsManager, CommandEventsManager>();
            services.AddSingleton<ICommandParser, CommandParser>();
            services.AddSingleton<Func<string, ICommand>>((IServiceProvider serviceProvider) => name =>
            {
                var commandType = CommandTypeResolver.Resolve(name);
                if (commandType == null)
                {
                    return null;
                }

                return (ICommand)serviceProvider.GetService(commandType);
            });

            foreach (var commandType in CommandTypeResolver.GetCommandTypes())
            {
                services.AddTransient(commandType);
            }
        }
    }
}
