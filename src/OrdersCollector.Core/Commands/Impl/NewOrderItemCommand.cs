using System;
using System.Collections.Generic;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Factories;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Resources;

namespace OrdersCollector.Core.Commands.Impl
{
    public class NewOrderItemCommand : Command<NewOrderItemResult>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IUserRepository userRepository;
        private readonly ISupplierRepository suppliersRepository;
        private readonly IOrderFactory orderFactory;
        private readonly IOrderItemFactory orderItemFactory;
        private readonly IUserFactory userFactory;

        public NewOrderItemCommand(ICommandEventsManager commandEventsManager, IOrderRepository orderRepository, IUserRepository userRepository, ISupplierRepository suppliersRepository, IOrderFactory orderFactory, IOrderItemFactory orderItemFactory, IUserFactory userFactory) : base(commandEventsManager)
        {
            this.orderRepository = orderRepository;
            this.userRepository = userRepository;
            this.suppliersRepository = suppliersRepository;
            this.orderFactory = orderFactory;
            this.orderItemFactory = orderItemFactory;
            this.userFactory = userFactory;
        }

        /// <summary>
        /// Name of the supplier.
        /// </summary>
        public string SupplierName { get; set; }

        /// <summary>
        /// Order content.
        /// </summary>
        public string OrderContent { get; set; }

        /// <summary>
        /// Name of the user for whom order item should be created. 
        /// User name and AuditInfo.InvokedBy can differ. 
        /// User can invoke operation on behalf of another user.
        /// </summary>
        public string UserDisplayName { get; set; }

        public override void SetArgs(params string[] args)
        {
            if (args != null && args.Length == 3)
            {
                SupplierName = args[0];
                UserDisplayName = args[1];
                OrderContent = args[2];
            }
            else
                throw new AppException(ErrorCodes.Common.InvalidCommandSyntax);
            
            base.SetArgs(args);
        }

        public override void Execute()
        {
            TypedResult = new NewOrderItemResult();
            var supplier = suppliersRepository.GetSupplierByName(SupplierName);
            if (supplier == null)
            {
                throw new AppException(ErrorCodes.Supplier.UnknownSupplier);
            }

            var order = orderRepository.GetActiveOrder(supplier);
            if (order == null)
            {
                order = orderFactory.Create(supplier);
                order.Supplier = supplier;
                orderRepository.Add(order);

                TypedResult.OrderCreated = true;
            }
            else
            {
                if (order.Items == null)
                {
                    order.Items = new List<OrderItem>();
                }
            }

            var user = GetUser();

            var orderItem = orderItemFactory.Create(order);
            orderItem.AuditInfo = AuditInfo;
            orderItem.User = user;
            orderItem.Content = OrderContent;

            order.Items.Add(orderItem);
            TypedResult.OrderItem = orderItem;
            base.Execute();
        }

        private User GetUser()
        {
            if (!string.IsNullOrWhiteSpace(UserDisplayName))
            {
                return GetUserByDisplayName();
            }

            return GetOrCreateUserForInvoker();
        }

        private User GetOrCreateUserForInvoker()
        {
            var user = userRepository.AsQueryable().FirstOrDefault(u => u.Name == AuditInfo.InvokedBy);
            if (user == null)
            {
                user = userFactory.Create();
                user.Name = AuditInfo.InvokedBy;
                user.FullName = AuditInfo.InvokedByName;
            }
            return user;
        }

        private User GetUserByDisplayName()
        {
            var user = userRepository.AsQueryable().FirstOrDefault(u => u.FullName == UserDisplayName);
            if (user == null)
            {
                throw new AppException(ErrorCodes.Account.UserNameNotExist);
            }

            return user;
        }
    }

    public class NewOrderItemResult : CommandResult
    {
        public bool OrderCreated { get; set; }

        public OrderItem OrderItem { get; set; }

        public override string GetMessage()
        {
            if (OrderCreated)
            {
                return String.Format("{0}, {1}", GetOrderCreatedMessage(), GetOrderItemCreatedMessage());
            }

            return GetOrderItemCreatedMessage();
        }

        private string GetOrderCreatedMessage()
        {
             return String.Format(Messages.OrderCreated, OrderItem.Order.Supplier.Name);
        }

        private string GetOrderItemCreatedMessage()
        {
            return String.Format(Messages.OrderItemCreated, OrderItem.Order.Supplier.Name, OrderItem.Order.OrderDate.ToShortDateString(), OrderItem.User.DisplayName, OrderItem.Content);
        }
    }
}
