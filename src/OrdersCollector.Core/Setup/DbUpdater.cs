using System.IO;
using System.Linq;
using OrdersCollector.Core.Persistence.Repositories;
using OrdersCollector.DAL;
using Microsoft.Extensions.Logging;

namespace OrdersCollector.Core.Setup
{
    /// <summary>
    /// Database updater.
    /// </summary>
    public interface IDbUpdater
    {
        /// <summary>
        /// Updates DB scheme.
        /// </summary>
        void Update();
    }

    public class DbUpdater : IDbUpdater
    {
        private readonly ILogger<DbUpdater> log;
        private readonly IIncrementalUpdateRepository incrementalUpdateRepository;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;
        private readonly IDbHelper dbHelper;

        private const string InsertUpdate = "insert into incrementalupdates (Name) values ({0})";
        public const string UpdatesDirectoryName = "db/updates";
        public const string InitialSchemaFile = "db/schema.sql";

        public DbUpdater(ILogger<DbUpdater> log, IIncrementalUpdateRepository incrementalUpdateRepository, IUnitOfWorkFactory unitOfWorkFactory, IDbHelper dbHelper)
        {
            this.log = log;
            this.incrementalUpdateRepository = incrementalUpdateRepository;
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.dbHelper = dbHelper;
        }

        public void Update()
        {
            using (var uow = unitOfWorkFactory.Create())
            {
                EnsureInitialized();
                EnsureUpdated();
            }
        }

        private void EnsureInitialized()
        {
            log.LogInformation("Checking if DB is initialized");

            if (!IsInitialized())
            {
                Initialize();
            }
        }

        private void EnsureUpdated()
        {
            log.LogInformation("Checking for DB incremental updates.");

            var files = GetSqlFiles();
            var scriptNames = files.Select(Path.GetFileNameWithoutExtension).ToArray();

            var scriptsRan = incrementalUpdateRepository.AsQueryable()
                .Select(i => i.Name)
                .ToArray();

            var toRun = scriptNames.Except(scriptsRan).ToArray();
            foreach (var scriptName in toRun)
            {
                log.LogInformation("Running sql script: {0}", scriptName);
                using (var transaction = dbHelper.BeginTransaction())
                {
                    RunScript(scriptName);
                    SetExecuted(scriptName);

                    transaction.Commit();
                }
            }
        }

        private void SetExecuted(string scriptName)
        {
            dbHelper.ExecuteSqlCommand(InsertUpdate, scriptName);
        }

        private void RunScript(string scriptName)
        {
            var content = GetSchemaUpdateScriptContent(scriptName);
             dbHelper.ExecuteSqlCommand(content);
        }

        private bool IsInitialized()
        {
            var result = dbHelper.ExecuteSqlScalarQuery(
                "select count(*) from sqlite_master where type='table' and name='IncrementalUpdates'"
                );

            return result > 0;
        }

        private void Initialize()
        {
            log.LogInformation("Running initial scheme sql script.");
            using (var transaction = dbHelper.BeginTransaction())
            {
                var content = GetInitialSchemaScriptContent();
                dbHelper.ExecuteSqlCommand(content);

                transaction.Commit();
            }
        }

        private string[] GetSqlFiles()
        {
            return Directory.GetFiles(UpdatesDirectoryName, "*.sql", SearchOption.TopDirectoryOnly)
                            .OrderBy(o => o).ToArray();
        }

        private string GetSchemaUpdateScriptContent(string name)
        {
            return File.ReadAllText(Path.Combine(UpdatesDirectoryName, name + ".sql"));
        }

        private string GetInitialSchemaScriptContent()
        {
            return File.ReadAllText(InitialSchemaFile);
        }
    }
}
