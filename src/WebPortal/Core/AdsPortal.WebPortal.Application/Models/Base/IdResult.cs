namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System;
    using MagicModels.Attributes;

    [RenderableClass]
    public class IdResult
    {
        public Guid Id { get; init; }

        public override string? ToString()
        {
            return Id.ToString();
        }
    }
}
