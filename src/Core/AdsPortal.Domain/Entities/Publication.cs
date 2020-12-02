namespace AdsPortal.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Domain.Abstractions.Base;

    public class Publication : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Title { get; set; } = string.Empty;
        public ushort Year { get; set; }
        public ushort ExternalAuthors { get; set; }

        public Guid JournalId { get; set; }
        public virtual Journal Journal { get; set; } = default!;

        public virtual ICollection<PublicationAuthor> PublicationAuthors { get; set; } = default!;
    }
}
