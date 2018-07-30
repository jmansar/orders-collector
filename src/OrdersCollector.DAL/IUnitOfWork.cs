using System;

namespace OrdersCollector.DAL
{
    /// <summary>
    /// Represents business transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();
    }
}
