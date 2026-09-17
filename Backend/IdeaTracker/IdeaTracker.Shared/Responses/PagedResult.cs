using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaTracker.Shared.Responses
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public PaginationMeta Pagination { get; set; }

        public PagedResult(List<T> items, PaginationMeta pagination)
        {
            Items = items;
            Pagination = pagination;
        }

        public static PagedResult<T> Create(List<T> items, int page, int pageSize, int totalCount) =>
            new(items, new PaginationMeta(page, pageSize, totalCount));
    }
}
