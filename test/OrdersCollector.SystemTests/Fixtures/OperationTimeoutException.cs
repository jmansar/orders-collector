using System;

namespace OrdersCollector.SystemTests.Fixtures
{
    public class OperationTimeoutException : Exception
    {
        public OperationTimeoutException(string message) : base(message)
        {
        }
    }
}
