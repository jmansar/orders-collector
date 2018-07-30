using System;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Resources;
using OrdersCollector.Utils.Format;

namespace OrdersCollector.Core.Commands.Impl
{
    public class GetOrderCommand : Command<GetOrderResult>
    {
        private readonly ISupplierRepository suppliersRepository;
        private readonly IOrderRepository orderRepository;

        public string SupplierName { get; set; }

        public GetOrderCommand(ICommandEventsManager commandEventsManager, ISupplierRepository suppliersRepository, IOrderRepository orderRepository) : base(commandEventsManager)
        {
            this.suppliersRepository = suppliersRepository;
            this.orderRepository = orderRepository;
        }

        public override void SetArgs(string[] args)
        {
            if (args != null && args.Length == 1)
                SupplierName = args[0];
            else
                throw new AppException(ErrorCodes.Common.InvalidCommandSyntax);

            base.SetArgs(args);
        }

        public override void Execute()
        {
            TypedResult = new GetOrderResult();

            var supplier = suppliersRepository.GetSupplierByName(SupplierName);
            if (supplier != null)
            {
                var order = orderRepository.GetActiveOrder(supplier);
                if (order != null)
                {
                    TypedResult.Order = order;
                }
                else
                {
                    throw new AppException(ErrorCodes.Order.NoActiveOrder, supplier.Name);
                }
            }
            else
            {
                throw new AppException(ErrorCodes.Supplier.UnknownSupplier);
            }
           
            base.Execute();
        }
    }

    public class GetOrderResult : CommandResult
    {

        public Order Order { get; set; }

        public override string GetMessage()
        {
            return String.Format(Messages.OrderFormat,
                DataFormatter.FormatDayDate(Order.OrderDate),
                Order.Supplier.Name,
                String.IsNullOrWhiteSpace(Order.Supplier.Phone) ? String.Empty : (String.Format(Messages.PhoneFormat, Order.Supplier.Phone) + "\n"),
                String.Join("\n", Order.Items.OrderBy(i => i.Content).Select(c => String.Format("- [{0}] **{1}**", c.User.DisplayName, c.Content)).ToList()),
                Order.Items.Count,
                Order.Operator != null ? (String.Format(Messages.OperatorFormat, Order.Operator.DisplayName)) + "\n" : String.Empty
                );
        }
    }
}
