using System;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Factories;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Resources;

namespace OrdersCollector.Core.Commands.Impl
{
    public class AddSupplierCommand : Command<AddSupplierResult>
    {
        private readonly ISupplierRepository suppliersRepository;
        private readonly ISupplierFactory supplierFactory;

        public AddSupplierCommand(ICommandEventsManager commandEventsManager, ISupplierRepository suppliersRepository, ISupplierFactory supplierFactory) : base(commandEventsManager)
        {
            this.suppliersRepository = suppliersRepository;
            this.supplierFactory = supplierFactory;
        }

        public string SupplierName { get; set; }

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
            TypedResult = new AddSupplierResult();

            CheckIfSupplierExist();

            var supplier = supplierFactory.Create();
            supplier.Name = SupplierName;

            suppliersRepository.Add(supplier);

            TypedResult.Supplier = supplier;

            base.Execute();
        }

        private void CheckIfSupplierExist()
        {
            var supplier = suppliersRepository.GetSupplierByName(SupplierName);
            if (supplier != null)
            {
                throw new AppException(ErrorCodes.Supplier.SupplierAlreadyExists);
            }
        }
    }

    public class AddSupplierResult : CommandResult
    {

        public Supplier Supplier { get; set; }

        public override string GetMessage()
        {
            if (Supplier != null)
            {
                return String.Format(Messages.SupplierAdded, Supplier.Name);
            }  

            return "";
        }
    }
}
