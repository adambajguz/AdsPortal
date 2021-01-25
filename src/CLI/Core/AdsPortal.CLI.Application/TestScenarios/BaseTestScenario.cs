namespace AdsPortal.CLI.Application.TestScenarios
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions.Execution;
    using Microsoft.Extensions.Logging;
    using Typin.Console;

    public abstract class BaseTestScenario : ITestScenario
    {
        public int CurrentTestId { get; private set; }
        public bool Started => CurrentTestId > 0;

        protected IConsole Console { get; }
        protected ILogger Logger { get; }

        protected BaseTestScenario(IConsole console, ILogger logger)
        {
            Console = console;
            Logger = logger;
        }

        public abstract string Name { get; }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);

        protected async Task Test(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
        {
            ++CurrentTestId;

            Stopwatch stopwatch = new();
            stopwatch.Start();

            string testName = action.Method.Name;
            Logger.LogInformation("Executing test #{Id} {Name}", CurrentTestId, testName);

            try
            {
                await action(cancellationToken);
                stopwatch.Stop();

                Logger.LogInformation("Test #{Id} {Name} finished successfully after {Elapsed}ms", CurrentTestId, testName, stopwatch.ElapsedMilliseconds);
                Console.Output.WithForegroundColor(ConsoleColor.Cyan, (o) => o.WriteLine($"Test #{CurrentTestId} '{testName}' finished successfully after {stopwatch.ElapsedMilliseconds}ms."));
            }
            catch (AssertionFailedException ex)
            {
                stopwatch.Stop();

                Logger.LogError(ex, "Assertion failed in test #{Id} {Name} after {Elapsed}ms", CurrentTestId, testName, stopwatch.ElapsedMilliseconds);
                Console.Output.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"Assertion failed in test #{CurrentTestId} '{testName}' after {stopwatch.ElapsedMilliseconds}ms."));

                throw;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                Logger.LogCritical(ex, "Fatal error in test #{Id} {Name} after {Elapsed}ms", CurrentTestId, Name, stopwatch.ElapsedMilliseconds);
                Console.Output.WithForegroundColor(ConsoleColor.Red, (o) => o.WriteLine($"Fatal error in test #{CurrentTestId} '{testName}' after {stopwatch.ElapsedMilliseconds}ms."));

                throw;
            }
        }
    }
}
