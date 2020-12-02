namespace AdsPortal.Infrastructure.Identity.UserManager
{
    using System.Threading;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Infrastructure.Identity.UserManager.Hasher;
    using FluentValidation;
    using Microsoft.Extensions.Options;

    public class UserManagerService : IUserManagerService
    {
        private readonly VersionedPasswordProvider _passwordHasher;

        public UserManagerService(IOptions<PasswordHasherSettings> settings)
        {
            _passwordHasher = new VersionedPasswordProvider(settings.Value);
        }

        public async Task<User> SetPassword(User user, string password, CancellationToken cancellationToken = default)
        {
            await new PasswordValidator().ValidateAndThrowAsync(password, cancellationToken: cancellationToken);

            user.Password = _passwordHasher.CreateHash(password);

            return user;
        }

        public async Task<bool> ValidatePassword(User user, string password, CancellationToken cancellationToken = default)
        {
            await new PasswordValidator().ValidateAndThrowAsync(password, cancellationToken: cancellationToken);

            return _passwordHasher.ValidatePassword(password, user.Password);
        }
    }
}
