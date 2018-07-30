using System;
using OrdersCollector.Resources;

namespace OrdersCollector.Core
{
    public class AppException : ApplicationException
    {
        public int ErrorCode { get; private set; }

        public string[] MessageArgs { get; set; }

        public AppException(int errorCode, params string[] messageArgs) : 
            base(String.Format(Errors.ResourceManager.GetString(String.Format("E_{0:00000}", errorCode)), messageArgs))
        {
            ErrorCode = errorCode;
            MessageArgs = messageArgs;
        }
    }
}
