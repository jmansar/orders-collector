using System;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Resources;
using OrdersCollector.Utils.Format;
using OrdersCollector.Utils.Randomization;

namespace OrdersCollector.Core.Commands.Impl
{

    public class PickOperatorCommand : Command<PickOperatorResult>
    {
        private readonly ISupplierRepository suppliersRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly IRandomizer randomizer;

        public PickOperatorCommand(
            ICommandEventsManager commandEventsManager, 
            ISupplierRepository suppliersRepository, 
            IOrderRepository orderRepository, 
            IUserRepository userRepository,
            IRandomizer randomizer)
            : base(commandEventsManager)
        {
            this.suppliersRepository = suppliersRepository;
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.randomizer = randomizer;
        }

        /// <summary>
        /// Supplier order name.
        /// </summary>
        public string TargetName { get; set; }

        public override void SetArgs(string[] args)
        {
            if (args != null)
            {
                if (args.Length == 1)
                    TargetName = args[0];
                else if (args.Length > 1)
                    throw new AppException(ErrorCodes.Common.InvalidCommandSyntax);
            }
            else
            {
                throw new AppException(ErrorCodes.Common.InvalidCommandSyntax);
            }

            base.SetArgs(args);
        }

        public override void Execute()
        {
            TypedResult = new PickOperatorResult();

            var users = new User[] { };

            var supplier = suppliersRepository.GetSupplierByName(TargetName);
            if (supplier == null)
            {
                throw new AppException(ErrorCodes.Supplier.UnknownSupplier);
            }

            var order = orderRepository.GetActiveOrder(supplier);
            if (order == null)
            {
                throw new AppException(ErrorCodes.Order.NoActiveOrder, supplier.Name);
            }

            TypedResult.Order = order;

            users = userRepository.GetUserOperatorPool(order).ToArray();
    
            if (users.Any())
            {
                TypedResult.User = randomizer.GetRandomItem(users);
                TypedResult.Users = users;
                TypedResult.Order.Operator = TypedResult.User;
            }

            base.Execute();
        }
    }

    public class PickOperatorResult : CommandResult
    {
        public User User { get; set; }

        public Order Order { get; set; }

        public User[] Users { get; set; }

        public override string GetMessage()
        {
            if (User != null)
            {
                var usersList = String.Join(", ", Users.Select(u => u.DisplayName).ToArray());
                return String.Format(Messages.RandomOperatorForOrder, Order.Supplier.Name, DataFormatter.FormatDayDate(Order.OrderDate), User.DisplayName, usersList);
            }
            
            return Messages.RandomNoUsers;
        }
    }
}
