using System;

namespace OrdersCollector.SystemTests
{
    public static class Config
    {
        public static TimeSpan DefaultOperationTimeout => TimeSpan.FromSeconds(5);
    }
}
