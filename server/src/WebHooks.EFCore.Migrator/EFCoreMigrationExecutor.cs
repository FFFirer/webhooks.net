using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebHooks.EFCore.Migrator
{
    public class EFCoreMigrationExecutor
    {
        private readonly ILogger _logger;
        private readonly WebHookMigrationDbContext _dbContext;

        public EFCoreMigrationExecutor(ILoggerFactory loggerFactory, WebHookMigrationDbContext dbContext)
        {
            _logger = loggerFactory.CreateLogger("Migrator");
            _dbContext = dbContext;
        }

        public void Run()
        {
            try
            {
                _dbContext.Database.Migrate();

                _logger.LogInformation("迁移完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "迁移失败");
            }
        }
    }
}
