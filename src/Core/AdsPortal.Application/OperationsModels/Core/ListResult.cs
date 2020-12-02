namespace AdsPortal.Application.OperationsModels.Core
{
    using System.Collections.Generic;
    using AdsPortal.Application.OperationsAbstractions;
    using AdsPortal.Domain.Mapping;

    public class ListResult<TResultEntry> : IOperationResult
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int Count { get; }
        public IList<TResultEntry> Entries { get; }

        public ListResult(List<TResultEntry> entries)
        {
            Count = entries.Count;
            Entries = entries;
        }
    }
}
