namespace AdsPortal.WebPortal.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [DeleteOperation]
    public class DeleteAdvertisement
    {
        public Guid Id { get; set; }
    }
}
