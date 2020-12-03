namespace AdsPortal.CLI.Interfaces
{
    using System.Threading.Tasks;
    using Typin.Console;

    public interface IDbMigrationsService
    {
        ValueTask Migrate(IConsole console);
        ValueTask Verify(IConsole console);
    }
}