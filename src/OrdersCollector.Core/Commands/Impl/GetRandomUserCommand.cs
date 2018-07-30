using System;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Resources;
using OrdersCollector.Utils.Randomization;

namespace OrdersCollector.Core.Commands.Impl
{
    public class GetRandomUserCommand : Command<GetRandomUserResult>
    {
        private readonly IOrderItemRepository orderItemsRepository;
        private readonly ISupplierRepository suppliersRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IRandomizer randomizer;

        public GetRandomUserCommand(
            ICommandEventsManager commandEventsManager, 
            IOrderItemRepository orderItemsRepository, 
            ISupplierRepository suppliersRepository, 
            IOrderRepository orderRepository,
            IRandomizer randomizer) : base(commandEventsManager)
        {
            this.orderItemsRepository = orderItemsRepository;
            this.suppliersRepository = suppliersRepository;
            this.orderRepository = orderRepository;
            this.randomizer = randomizer;
        }

        /// <summary>
        /// If set then random user will be return from users that made order to specified supplier.
        /// </summary>
        public string SupplierName { get; set; }

        public override void SetArgs(string[] args)
        {
            if (args != null)
            {
                if (args.Length == 1)
                    SupplierName = args[0];
                else if (args.Length > 1)
                    throw new AppException(ErrorCodes.Common.InvalidCommandSyntax);
            }

            base.SetArgs(args);
        }

        public override void Execute()
        {
            TypedResult = new GetRandomUserResult();

            User[] users = new User[]{};

            var supplier = suppliersRepository.GetSupplierByName(SupplierName);
            if (supplier != null)
            {
                var order = orderRepository.GetActiveOrder(supplier);
                if (order == null)
                {
                    throw new AppException(ErrorCodes.Order.NoActiveOrder, supplier.Name);
                }

                TypedResult.Order = order;
                users = order.Items.Select(i => i.User).Distinct().ToArray();
            }
            else
            {
                var today = DateTime.Now.Date;
                users = orderItemsRepository.AsQueryable()
                    .Where(u => u.Order.OrderDate == today)
                    .Select(i => i.User).Distinct().ToArray();
            }

            if (users.Any())
            {
                TypedResult.User = randomizer.GetRandomItem(users);
                TypedResult.Users = users;
            }

            base.Execute();
        }
    }

    public class GetRandomUserResult : CommandResult
    {
        public User User { get; set; }

        public Order Order { get; set; }

        public User[] Users { get; set; }

        public override string GetMessage()
        {
            if (User != null)
            {
                var usersList = String.Join(", ", Users.Select(u => u.DisplayName).ToArray());
                if (Order != null)
                {
                    return String.Format(Messages.RandomUserForOrder, User.DisplayName, Order.Supplier.Name, Order.OrderDate.ToShortDateString(), usersList);
                }
                else
                {
                    return String.Format(Messages.RandomUserActiveOrders, User.DisplayName, usersList);
                }
            }
            else
            {
                return Messages.RandomNoUsers;
            }
        }
    }
}
