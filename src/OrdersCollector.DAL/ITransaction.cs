using System;

namespace OrdersCollector.DAL
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
