namespace AdsPortal.Application.Interfaces.Identity
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Entities;

    public interface IUserManagerService
    {
        Task<User> SetPassword(User user, string password, CancellationToken cancellationToken = default);
        Task<bool> ValidatePassword(User user, string password, CancellationToken cancellationToken = default);
    }
}
