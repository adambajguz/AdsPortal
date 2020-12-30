namespace AdsPortal.Persistence.Repository
{
    using System.Linq;
    using System.Threading.Tasks;
    using AdsPortal.Application.Interfaces.Identity;
    using AdsPortal.Application.Interfaces.Persistence.Repository;
    using AdsPortal.Domain.Entities;
    using AdsPortal.Persistence.Interfaces.DbContext;
    using AdsPortal.Persistence.Repository.Generic;
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
                return false;

            return true;
        }
    }
}
