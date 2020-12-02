namespace AdsPortal.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using AdsPortal.Domain.Abstractions.Base;

    public class Author : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string ORCID { get; set; } = string.Empty;

        public Guid DepartmentId { get; set; }
        public Guid DegreeId { get; set; }

        public virtual Department Department { get; set; } = default!;
        public virtual Degree Degree { get; set; } = default!;

        public virtual ICollection<PublicationAuthor> PublicationAuthors { get; set; } = default!;
    }
}
