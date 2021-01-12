namespace AdsPortal.CLI.Application.Commands
{
    using System;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Typin;
    using Typin.Attributes;
    using Typin.Console;
    using Typin.Utilities;

    [Command("tools database drop", Description = "Drops database.")]
    public class DropDatabaseCommand : ICommand
    {
        [CommandOption("conn-string", 'c', Description = "Connection string.", FallbackVariableName = "CONNECTION_STRING")]
        public string ConnectionString { get; init; } = @"Data Source=.\SQLEXPRESS;Integrated Security=True;Database=master";

        [CommandOption("name", 'n', Description = "Database name.", FallbackVariableName = "DB_NAME")]
        public string DatabaseName { get; init; } = "AdsPortal_Test";

        private ILogger _logger;

        public DropDatabaseCommand(ILogger<DropDatabaseCommand> logger)
        {
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                CancellationToken cancellationToken = console.GetCancellationToken();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    bool dbExists = await CheckDatabaseExists(connection, cancellationToken);

                    if (dbExists)
                    {
                        using (SqlCommand command = new SqlCommand($"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{DatabaseName}];", connection))
                        {
                            await connection.OpenAsync(cancellationToken);
                            await command.ExecuteNonQueryAsync(cancellationToken);
                            console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"Database '{DatabaseName}' deleted successfully."));
                        }
                    }
                    else
                    {
                        console.Error.WithForegroundColor(ConsoleColor.DarkYellow, (e) => e.WriteLine($"Database '{DatabaseName}' does not exist."));
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, $"Failed to drop database '{DatabaseName}'.");

                console.Error.WithForegroundColor(ConsoleColor.Red, (e) => e.WriteLine("Error occured. Details:"));
                ExceptionFormatter.WriteException(console.Error, ex);
            }
        }

        private async Task<bool> CheckDatabaseExists(SqlConnection connection, CancellationToken cancellationToken = default)
        {
            using (SqlCommand command = new SqlCommand($"SELECT database_id FROM sys.databases WHERE Name = '{DatabaseName}';", connection))
            {
                await connection.OpenAsync(cancellationToken);
                object? result = await command.ExecuteScalarAsync(cancellationToken);

                if (result is not null && int.TryParse(result.ToString(), out int databaseID))
                {
                    return (databaseID > 0);
                }
            }

            return false;
        }
    }
}

