namespace AdsPortal.CLI.Application.TestScenarios
{
    using System.Threading;
    using System.Threading.Tasks;
    using Typin.Console;

    public abstract class BaseTestScenario : ITestScenario
    {
        protected IConsole Console { get; }

        public BaseTestScenario(IConsole console)
        {
            Console = console;
        }

        public abstract string Name { get; }

        public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
