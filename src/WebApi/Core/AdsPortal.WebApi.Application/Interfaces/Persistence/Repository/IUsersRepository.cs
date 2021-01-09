namespace AdsPortal.WebApi.Application.Interfaces.Persistence.Repository
{
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository.Generic;
    using AdsPortal.WebApi.Domain.Entities;

    public interface IUsersRepository : IGenericRelationalRepository<User>
    {
        Task<bool> IsEmailInUseAsync(string? email);
    }
}
