namespace AdsPortal.WebPortal.Models.Base
{
    using MagicModels.Attributes;

    public abstract class GetPagedListQuery
    {
        public int Page { get; set; }

        [RenderableProperty(DisplayName = "Entires per page")]
        public int EntiresPerPage { get; set; }
    }
}
