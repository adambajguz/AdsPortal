using System.Threading.Tasks;

namespace AdsPortal.CLI.Interfaces
{
    public interface ICliRuntimeService
    {
        bool IsInteractive { get; }

        void Exit();
        Task ExitAsync();

        void ValidateInteractiveAndThrow();
    }
}