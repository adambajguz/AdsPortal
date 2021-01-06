namespace AdsPortal.WebPortal.Application.Models
{
    using System;
    using System.Collections.Generic;
    using MagicModels.Attributes;

    [RenderableClass]
    public class TestModel
    {
        public Guid Id { get; init; }

        public DateTime CreatedOn { get; init; }
        public Guid? CreatedBy { get; init; }
        public DateTime LastSavedOn { get; init; }
        public Guid? LastSavedBy { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;

        [RenderablePropertyIgnore]
        public IList<InnerTestModel> Advertisements { get; set; } = default!;
    }

    [RenderableClass]
    public class InnerTestModel
    {
        public Guid Id { get; init; }
        public string Text { get; init; } = string.Empty;
    }
}
