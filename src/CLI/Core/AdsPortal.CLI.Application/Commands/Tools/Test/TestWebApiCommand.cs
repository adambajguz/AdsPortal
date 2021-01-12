namespace AdsPortal.CLI.Application.Commands.Tools.Test
{
    using System.Threading.Tasks;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;

    [Command("tools test WebApi", Description = "Tests 'AdsPortal.WebApi'.")]
    public class TestWebApiCommand : ICommand
    {
        [CommandOption("conn-string", 'c', Description = "Connection string.", FallbackVariableName = "CONNECTION_STRING")]
        public string ConnectionString { get; init; } = @"Data Source=.\SQLEXPRESS;Integrated Security=True;Database=master";

        [CommandOption("name", 'n', Description = "Database name.", FallbackVariableName = "DB_NAME")]
        public string DatabaseName { get; init; } = "AdsPortal_Test";

        private readonly ICliApplicationLifetime _cliApplicationLifetime;
        private readonly ICliCommandExecutor _cliCommandExecutor;

        public TestWebApiCommand(ICliApplicationLifetime cliApplicationLifetime, ICliCommandExecutor cliCommandExecutor)
        {
            _cliApplicationLifetime = cliApplicationLifetime;
            _cliCommandExecutor = cliCommandExecutor;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            await _cliCommandExecutor.ExecuteCommandAsync(@$"tools database drop -c ""{ConnectionString}"" -n ""{DatabaseName}""");

            _cliApplicationLifetime.RequestStop();
        }
    }
}
