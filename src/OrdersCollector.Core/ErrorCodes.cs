namespace OrdersCollector.Core
{
    public static class ErrorCodes
    {
        public static class Common
        {
            public const int UnknownError = 0;
            public const int UnknownCommand = 1;
            public const int InvalidCommandSyntax = 2;
            public const int InvalidParameterFormat = 3;
        }

        public static class Supplier
        {
            public const int UnknownSupplier = 100;
            public const int SupplierAlreadyExists = 101;
            
        }

        public static class Order
        {
            public const int NoActiveOrder = 200;
        }

        public static class OrderItem
        {
            public const int NoOrderItemToDelete = 400;
        }

        public static class Account
        {
            public const int OpenIdAccountAlreadyExists = 500;
            public const int UserNameNotExist = 501;
            public const int ActivationCodeInvalid = 502;
            public const int UserExists = 503;
        }
    }
}
