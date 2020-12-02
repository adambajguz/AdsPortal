namespace AdsPortal.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Domain.Abstractions.Base;

    public class Degree : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual ICollection<Author> Authors { get; set; } = default!;
    }
}
