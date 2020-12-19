namespace MagicCRUD.Configurations
{
    using System;
    using System.Collections.Generic;

    public class MagicCRUDConfiguration
    {
        public string BaseApiPath { get; init; } = string.Empty;
        public IReadOnlyDictionary<Type, OperationGroupConfiguration> OperationGroups { get; init; } = default!;
    }
}