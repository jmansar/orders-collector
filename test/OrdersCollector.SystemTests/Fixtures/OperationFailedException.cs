using System;

namespace OrdersCollector.SystemTests.Fixtures
{
    public class OperationFailedException : Exception
    {
        public OperationFailedException(string message) : base(message)
        {
        }
    }
}
