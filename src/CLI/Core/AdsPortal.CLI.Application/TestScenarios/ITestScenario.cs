namespace AdsPortal.CLI.Application.TestScenarios
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ITestScenario
    {
        string Name { get; }

        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
