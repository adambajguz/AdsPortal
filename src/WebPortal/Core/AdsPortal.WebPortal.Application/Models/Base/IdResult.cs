namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System;
    using MagicOperations.Attributes;

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
