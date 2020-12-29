namespace AdsPortal.ManagementUI.Models.Advertisement
{
    using System;
    using AdsPortal.ManagementUI.Models;
    using MagicOperations.Attributes;

    [OperationGroup(OperationGroups.Advertisement)]
    [DeleteOperation]
    public class DeleteAdvertisement
    {
        public Guid Id { get; init; }
    }
}
