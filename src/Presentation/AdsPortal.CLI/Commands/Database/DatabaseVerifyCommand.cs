namespace AdsPortal.CLI.Commands.Database
{
    using System.Threading.Tasks;
    using AdsPortal.CLI.Interfaces;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("database verify", Description = "Verify Entity Framework migrations.")]
    public class DatabaseVerifyCommand : ICommand
    {
        private readonly IDbMigrationsService _dbMigrationsService;

        public DatabaseVerifyCommand(IDbMigrationsService dbMigrationsService)
        {
            _dbMigrationsService = dbMigrationsService;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _dbMigrationsService.Verify(console);
        }
    }
}
