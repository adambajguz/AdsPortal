namespace AdsPortal.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Domain.Abstractions.Base;

    public class Journal : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string NameAlt { get; set; } = string.Empty;
        public string ISSN { get; set; } = string.Empty;
        public string EISSN { get; set; } = string.Empty;

        public double Points { get; set; }

        public virtual List<string> Disciplines { get; set; } = new List<string>();

        public virtual ICollection<Publication> Publications { get; set; } = default!;
    }
}
