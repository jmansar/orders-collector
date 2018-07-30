namespace OrdersCollector.DAL
{
    public interface IDbHelper
    {
        /// <summary>
        /// Open explicit transaction. Use only when executing sql commands.
        /// </summary>
        /// <returns></returns>
        ITransaction BeginTransaction();

        int ExecuteSqlCommand(string sql, params object[] parameters);

        long ExecuteSqlScalarQuery(string sql);
    }
}
