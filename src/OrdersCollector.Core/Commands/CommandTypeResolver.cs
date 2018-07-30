using System;
using System.Collections.Generic;
using OrdersCollector.Core.Commands.Impl;

namespace OrdersCollector.Core.Commands
{
    public static class CommandTypeResolver
    {
        private static readonly IDictionary<string, Type> commandTypesDictionary = new Dictionary<string, Type>
        {
            // TODO: add multi-language support for commands
            ["nowyDostawca"] = typeof(AddSupplierCommand),
            ["usunMojeZam"] = typeof(DeleteOrderItemCommand),
            ["lista"] = typeof(GetOrderCommand),
            ["losujOsobe"] = typeof(GetRandomUserCommand),
            ["dostawcy"] = typeof(GetSuppliersCommand),
            ["pomoc"] = typeof(HelpCommand),
            ["dodaj"] = typeof(NewOrderItemCommand),
            ["losuj"] = typeof(PickOperatorCommand),
            ["wersja"] = typeof(VersionInfoCommand),
        };

        public static IEnumerable<Type> GetCommandTypes()
        {
            return commandTypesDictionary.Values;
        }

        public static Type Resolve(string name)
        {
            if (commandTypesDictionary.ContainsKey(name))
            {
                return commandTypesDictionary[name];
            }

            return null;
        }
    }
}
