using System;
using System.Collections.Generic;
using System.Linq;
using OrdersCollector.Core.Models;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.Resources;

namespace OrdersCollector.Core.Commands.Impl
{
    public class GetSuppliersCommand : Command<GetSuppliersCommandResult>
    {
        public string SupplierName { get; set; }

        private readonly ISupplierRepository supplierRepository;

        public GetSuppliersCommand(ICommandEventsManager commandEventsManager, ISupplierRepository supplierRepository) : base(commandEventsManager)
        {
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
        }

        public override void Execute()
        {
            TypedResult = new GetSuppliersCommandResult();

            if (String.IsNullOrWhiteSpace(SupplierName))
            {
                TypedResult.Suppliers = supplierRepository.GetAllSuppliers();
            }
            else
            {
                TypedResult.Suppliers = supplierRepository.GetSuppliersStartWith(SupplierName);
            }

            base.Execute();
        }
    }

    public class GetSuppliersCommandResult : CommandResult
    {
        public override string GetMessage()
        {
            return String.Format(Messages.SuppliersList,
                                 String.Join("\n", Suppliers.OrderBy(s => s.Name).Select(FormatSupplier)));
        }

        private string FormatSupplier(Supplier supplier)
        {
            return String.Join("\n", new[]
                {
                    String.Format("* {0}", supplier.Name),
                    String.Join("\n", supplier.SupplierAliases.Select(s => String.Format("*** {0}", s.Name)))
                }.Where(l => !String.IsNullOrWhiteSpace(l)));
        }

        public IList<Supplier> Suppliers { get; set; }
    }
}
