namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Base;

    public class PublicationAuthor : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public Guid AuthorId { get; set; }
        public Guid PublicationId { get; set; }

        public virtual Author Author { get; set; } = default!;
        public virtual Publication Publication { get; set; } = default!;
    }
}
