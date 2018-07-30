using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace OrdersCollector.DAL.EF
{
    public class DbHelper : IDbHelper
    {
        private readonly IContextProvider contextProvider;

        public DbHelper(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public ITransaction BeginTransaction()
        {
            var context = contextProvider.GetCurrentContext();
            var transaction = context.Database.BeginTransaction();
            return new TransactionWrapper(transaction);
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return contextProvider.GetCurrentContext()
                .Database.ExecuteSqlCommand(sql, parameters);
        }

        public long ExecuteSqlScalarQuery(string sql)
        {
            var context = contextProvider.GetCurrentContext();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.Connection.Open();

                return (long) command.ExecuteScalar();
            }
        }
    }

    public class TransactionWrapper : ITransaction
    {
        private IDbContextTransaction transaction;

        public TransactionWrapper(IDbContextTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Dispose()
        {
            transaction.Dispose();
            transaction = null;
        }

        public void Commit()
        {
            transaction.Commit(); 
        }

        public void Rollback()
        {
            transaction.Rollback();
        }
    }
}
