using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Shared.Paging
{
    public static class PagingQueryExtension
    {
        public static IQueryable<T> Paging<T, TPagingQuery>(this IQueryable<T> query, TPagingQuery pagingQuery) where TPagingQuery : PagingQuery
        {
            if (pagingQuery.Page > 0 && pagingQuery.PageSize > 0)
            {
                return query.Skip((pagingQuery.Page - 1) * pagingQuery.PageSize).Take(pagingQuery.PageSize);
            }
            return query;
        }
    }
}
