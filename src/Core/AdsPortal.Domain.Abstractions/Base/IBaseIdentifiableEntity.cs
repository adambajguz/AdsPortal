namespace AdsPortal.Domain.Abstractions.Base
{
    using System;

    public interface IBaseIdentifiableEntity
    {
        Guid Id { get; set; }
        //Guid OwnerId { get; set; }
    }
}
