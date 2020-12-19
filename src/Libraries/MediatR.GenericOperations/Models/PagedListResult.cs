namespace MediatR.GenericOperations.Models
{
    using System;
    using System.Collections.Generic;
    using MediatR.GenericOperations.Abstractions;
    using MediatR.GenericOperations.Mapping;

    public class PagedListResult<TResultEntry> : IOperationResult
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int CurrentPageNumber { get; }
        public int EntiresPerPage { get; }
        public int LastPage { get; }

        public int Seen { get; }
        public int Count { get; }
        public int Left { get; }
        public int TotalCount { get; }

        public IList<TResultEntry> Entries { get; }

        public PagedListResult(int currentPageNumber, int entriesPerPage, int totalCount, List<TResultEntry> entries)
        {
            CurrentPageNumber = currentPageNumber;
            EntiresPerPage = entriesPerPage;
            LastPage = (int)Math.Ceiling(totalCount / (double)entriesPerPage) - 1;

            int seenTmp = currentPageNumber * entriesPerPage;
            Seen = seenTmp > totalCount ? totalCount : seenTmp;

            Count = entries.Count;
            Left = totalCount - Seen - Count;
            TotalCount = totalCount;

            Entries = entries;
        }
    }
}
