namespace AdsPortal.Domain.Abstractions.Base
{
    using System;

    public interface IEntityCreation
    {
        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}