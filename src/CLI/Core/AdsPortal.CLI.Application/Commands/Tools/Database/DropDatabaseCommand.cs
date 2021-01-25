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

        private readonly ILogger _logger;

        public DropDatabaseCommand(ILogger<DropDatabaseCommand> logger)
        {
            _logger = logger;
        }

        public async ValueTask ExecuteAsync(IConsole console)
        {
            try
            {
                CancellationToken cancellationToken = console.GetCancellationToken();

                using (SqlConnection connection = new(ConnectionString))
                {
                    await connection.OpenAsync(cancellationToken);

                    bool dbExists = await CheckDatabaseExists(connection, cancellationToken);
                    if (dbExists)
                    {
                        _logger.LogDebug("Database {Name} exists -> execution drop", DatabaseName);

                        using (SqlCommand command = new($"ALTER DATABASE [{DatabaseName}] SET OFFLINE WITH ROLLBACK IMMEDIATE;", connection))
                        {
                            console.Output.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write("[1/4] "));
                            await command.ExecuteNonQueryAsync(cancellationToken);
                            console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"Database '{DatabaseName}' set offline."));

                            _logger.LogInformation("Database {Name} set offline", DatabaseName);
                        }

                        using (SqlCommand command = new($"ALTER DATABASE [{DatabaseName}] SET ONLINE WITH ROLLBACK IMMEDIATE;", connection))
                        {
                            console.Output.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write("[2/4] "));
                            await command.ExecuteNonQueryAsync(cancellationToken);
                            console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"Database '{DatabaseName}' set online."));

                            _logger.LogInformation("Database {Name} set online", DatabaseName);
                        }

                        using (SqlCommand command = new($"ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;", connection))
                        {
                            console.Output.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write("[3/4] "));
                            await command.ExecuteNonQueryAsync(cancellationToken);
                            console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"Database '{DatabaseName}' connections closed."));

                            _logger.LogInformation("Database {Name} connections closed", DatabaseName);
                        }

                        using (SqlCommand command = new($"DROP DATABASE [{DatabaseName}];", connection))
                        {
                            console.Output.WithForegroundColor(ConsoleColor.Magenta, (o) => o.Write("[4/4] "));
                            await command.ExecuteNonQueryAsync(cancellationToken);
                            console.Output.WithForegroundColor(ConsoleColor.Green, (o) => o.WriteLine($"Database '{DatabaseName}' deleted successfully."));

                            _logger.LogInformation("Database {Name} deleted", DatabaseName);
                        }
                    }
                    else
                    {
                        console.Error.WithForegroundColor(ConsoleColor.DarkYellow, (e) => e.WriteLine($"Database '{DatabaseName}' does not exist."));
                        _logger.LogInformation("Database {Name} does not exist", DatabaseName);
                    }

                    await connection.CloseAsync();
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
            using (SqlCommand command = new($"SELECT database_id FROM sys.databases WHERE Name = '{DatabaseName}';", connection))
            {
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

