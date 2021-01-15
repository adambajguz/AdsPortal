namespace MediatR.GenericOperations.Models
{
    using System;
    using System.Collections.Generic;
    using AutoMapper.Extensions;
    using MediatR.GenericOperations.Abstractions;

    public class PagedListResult<TResultEntry> : ListResult<TResultEntry>
        where TResultEntry : class, IIdentifiableOperationResult, ICustomMapping
    {
        public int CurrentPageNumber { get; }
        public int EntiresPerPage { get; }
        public int LastPage { get; }

        public int Seen { get; }
        public int Left { get; }
        public int TotalCount { get; }

        public PagedListResult(int currentPageNumber, int entriesPerPage, int totalCount, List<TResultEntry> entries) :
            base(entries)
        {
            CurrentPageNumber = currentPageNumber;
            EntiresPerPage = entriesPerPage;
            LastPage = (int)Math.Ceiling(totalCount / (double)entriesPerPage) - 1;

            int seenTmp = currentPageNumber * entriesPerPage;
            Seen = seenTmp > totalCount ? totalCount : seenTmp;

            Left = totalCount - Seen - Count;
            TotalCount = totalCount;
        }
    }
}
