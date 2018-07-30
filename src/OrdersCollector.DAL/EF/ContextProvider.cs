using System;
using OrdersCollector.Utils.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace OrdersCollector.DAL.EF
{
    /// <summary>
    /// Provides current db context.
    /// </summary>
    public interface IContextProvider
    {
        /// <summary>
        /// Returns current context.
        /// </summary>
        DbContext GetCurrentContext();
    }

    public class ContextProvider : IContextProvider
    {
        /// <summary>
        /// Returns current context.
        /// </summary>
        public DbContext GetCurrentContext()
        {
            if (UnitOfWork.Current == null)
                throw new InvalidOperationException("UnitOfWork scope not exists.");

            return UnitOfWork.Current.Context;
        }
    }
}
