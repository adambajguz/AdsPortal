namespace AdsPortal.Application.Interfaces.Persistence.Repository
{
    using AdsPortal.Application.Interfaces.Persistence.Repository.Generic;
    using Domain.Entities;

    public interface IPublicationAuthorsRepository : IGenericRelationalRepository<PublicationAuthor>
    {

    }
}
