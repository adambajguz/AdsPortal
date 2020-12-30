namespace AdsPortal.Persistence.Interfaces.DbContext.Generic
{
    using Microsoft.EntityFrameworkCore;

    public interface IGenericRelationalDbContext
    {
        DbContext Provider { get; }
    }
}
