namespace AdsPortal.WebApi.Persistence.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.WebApi.Application.Interfaces.Identity;
    using AdsPortal.WebApi.Application.Interfaces.Persistence.Repository;
    using AdsPortal.WebApi.Domain.Entities;
    using AdsPortal.WebApi.Persistence.Interfaces.DbContext;
    using AdsPortal.WebApi.Persistence.Repository.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class UsersRepository : GenericRelationalRepository<User>, IUsersRepository
    {
        public UsersRepository(ICurrentUserService currentUserService,
                               IRelationalDbContext context,
                               IMapper mapper) : base(currentUserService, context, mapper)
        {

        }

        public async Task<bool> IsEmailInUseAsync(string? email)
        {
            User? user = await DbSet.Where(x => x.Email.Equals(email)).SingleOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            return true;
        }
    }
}
