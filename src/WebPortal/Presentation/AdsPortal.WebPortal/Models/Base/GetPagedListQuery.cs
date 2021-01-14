namespace AdsPortal.WebPortal.Models.Base
{
    using MagicModels.Attributes;

    public abstract class GetPagedListQuery
    {
        public int Page { get; init; }

        [RenderableProperty(DisplayName = "Entires per page")]
        public int EntiresPerPage { get; init; }
    }
}
