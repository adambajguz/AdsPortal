namespace AdsPortal.Domain.Entities
{
    using System;
    using AdsPortal.Domain.Abstractions.Base;

    public class Advertisement : IBaseRelationalEntity, IEntityInfo
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }

        public DateTime LastSavedOn { get; set; }
        public Guid? LastSavedBy { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public bool IsPublished { get; set; }
        public DateTime VisibleTo { get; set; }

        public Guid? CoverImageId { get; set; }
        public virtual MediaItem? CoverImage { get; set; } = default!;

        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; } = default!;

        public Guid AuthorId { get; set; }
        public virtual User Author { get; set; } = default!;
    }
}
