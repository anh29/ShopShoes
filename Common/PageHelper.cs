using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourProject.Common
{
    public static class PageHelper
    {
        public static IPagedList<T> GetPagedList<T>(IEnumerable<T> items, int? page, int pageSize)
        {
            var pageIndex = page ?? 1;
            return items.ToPagedList(pageIndex, pageSize);
        }
    }
}
