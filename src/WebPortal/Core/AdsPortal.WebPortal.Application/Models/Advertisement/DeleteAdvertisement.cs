namespace AdsPortal.WebPortal.Application.Models.Advertisement
{
    using System;
    using AdsPortal.WebPortal.Application.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [DeleteOperation]
    public class DeleteAdvertisement
    {
        public Guid Id { get; init; }
    }
}