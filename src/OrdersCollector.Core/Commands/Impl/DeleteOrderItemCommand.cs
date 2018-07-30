using System;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.DAL.EF;
using OrdersCollector.Resources;

namespace OrdersCollector.Core.Commands.Impl
{
    /// <summary>
    /// Delete order item command.
    /// Allows to delete last order item from the active order.
    /// </summary>
    public class DeleteOrderItemCommand : Command<DeleteOrderItemResult>
    {

        private readonly IOrderItemRepository orderItemRepository;
        private readonly IOrderRepository orderRepository;
        private readonly ISupplierRepository supplierRepository;

        /// <summary>
        /// Name of the supplier of the order.
        /// </summary>
        public string SupplierName { get; set; }

        // TODO: add support for specifying display name
        /// <summary>
        /// Name of the user associated with the item to delete.
        /// </summary>
        public string UserName { get; set; }

        public DeleteOrderItemCommand(ICommandEventsManager commandEventsManager, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, ISupplierRepository supplierRepository) : base(commandEventsManager)
        {
            this.orderItemRepository = orderItemRepository;
            this.orderRepository = orderRepository;
            this.supplierRepository = supplierRepository;
        }

        public override void SetArgs(params string[] args)
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
            OrderItem orderItem;

            TypedResult = new DeleteOrderItemResult();
            UserName = AuditInfo.InvokedBy;

            if (!String.IsNullOrWhiteSpace(SupplierName))
            {
                orderItem = GetLastOrderItemBySupplierName(SupplierName);
            }
            else
            {
                orderItem = GetLastOrderItem(UserName);
            }

            if (orderItem == null)
            {
                throw new AppException(ErrorCodes.OrderItem.NoOrderItemToDelete);
            }

            TypedResult.OrderContent = orderItem.Content;
            TypedResult.Order = orderItem.Order;
            TypedResult.User = orderItem.User;

            orderItemRepository.Delete(orderItem);

            base.Execute();
        }

        private OrderItem GetLastOrderItem(string userName)
        {
            return orderItemRepository.AsQueryable()
                                                .EagerFetch(i => i.Order)
                                                .EagerFetch(i => i.Order.Supplier)
                                                .EagerFetch(i => i.User)
                                                .Where(i => i.User.Name == userName)
                                                .Where(i => i.Order.OrderDate == DateTime.Today)
                                                .OrderByDescending(i => i.Id)
                                                .FirstOrDefault();
        }

        private OrderItem GetLastOrderItemBySupplierName(string supplierName)
        {
            OrderItem orderItem;
            var supplier = supplierRepository.GetSupplierByName(supplierName);
            if (supplier == null)
            {
                throw new AppException(ErrorCodes.Supplier.UnknownSupplier);
            }

            var order = orderRepository.GetActiveOrder(supplier);
            if (order == null)
            {
                throw new AppException(ErrorCodes.Order.NoActiveOrder, supplier.Name);
            }

            orderItem = order.Items
                             .Where(i => i.User.Name == UserName)
                             .OrderByDescending(i => i.Id)
                             .FirstOrDefault();
            return orderItem;
        }
    }

    public class DeleteOrderItemResult : CommandResult
    {
        public string OrderContent { get; set; }

        public User User { get; set; }

        public Order Order { get; set; }

        public override string GetMessage()
        {
            return String.Format(Messages.OrderItemDeleted,
                                 OrderContent,
                                 Order.Supplier.Name,
                                 User.DisplayName);
        }
    }
}
