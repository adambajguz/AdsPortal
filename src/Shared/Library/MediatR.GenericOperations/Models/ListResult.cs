namespace MediatR.GenericOperations.Models
{
    using System.Collections.Generic;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class ListResult<TResultEntry> : IOperationResult
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int Count => Entries.Count;
        public IReadOnlyList<TResultEntry> Entries { get; }

        public ListResult(List<TResultEntry> entries)
        {
            Entries = entries;
        }
    }
}
