namespace MediatR.GenericOperations.Models
{
    using System.Collections.Generic;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

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
