using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebHooks.Shared.Paging
{
    public class PagingResult<T> where T : class
    {
        public List<T> Rows { get; set; } = new List<T>();
        public int Total { get; set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public PagingResult() { }

        public PagingResult(PagingQuery query)
        {
            Page = query.Page;
            PageSize = query.PageSize;
        }
    }
}
