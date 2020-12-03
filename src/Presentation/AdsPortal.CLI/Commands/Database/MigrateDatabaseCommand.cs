namespace AdsPortal.CLI.Commands.Database
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("database migrate", Description = "Apply Entity Framework migrations.")]
    public class MigrateDatabaseCommand : ICommand
    {
        private readonly IDbMigrationsService _dbMigrationsService;

        public MigrateDatabaseCommand(IDbMigrationsService dbMigrationsService)
        {
            _dbMigrationsService = dbMigrationsService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _dbMigrationsService.Migrate(console);
        }
    }
}
