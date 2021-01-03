namespace AdsPortal.WebPortal.Application.Models.Base
{
    using System;

    public class IdResult
    {
        public Guid Id { get; init; }

        public override string? ToString()
        {
            return Id.ToString();
        }
    }
}
