namespace AdsPortal.WebApi.Domain.Interfaces.Repository
{
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Domain.Interfaces.Repository.Generic;

    public interface IUsersRepository : IGenericRelationalRepository<User>
    {
        Task<bool> IsEmailInUseAsync(string? email);
    }
}
