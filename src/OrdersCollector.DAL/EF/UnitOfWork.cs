using System;
using OrdersCollector.DAL;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace OrdersCollector.Utils.DAL.EF
{
    /// <summary>
    /// Groups set of tasks into a single group of transactional work.
    /// </summary>
    internal class UnitOfWork : IDisposable, IUnitOfWork
    {
        private static readonly AsyncLocal<UnitOfWork> current = new AsyncLocal<UnitOfWork>();

        /// <summary>
        /// Current scope.
        /// </summary>
        public static UnitOfWork Current
        {
            get { return current.Value; }
        }

        private UnitOfWork parent;

        /// <summary>
        /// Current 
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnitOfWork(DbContext context)
        {
            if (current.Value != null)
            {
                parent = current.Value;
            }

            current.Value = this;
            Context = context;
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            if (parent != null)
            {
                current.Value = parent;
            }
        }

        /// <summary>
        /// Commits UoW.
        /// </summary>
        public void Commit()
        {
            Context.SaveChanges();
        }

        /// <summary>
        /// Discard changes.
        /// </summary>
        public void Rollback()
        {
            Context.Dispose();
            Context = null;
        }
    }
}
