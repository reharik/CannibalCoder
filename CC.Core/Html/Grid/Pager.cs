using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CC.Core.Html.Grid
{
    public class GridItemsDetail
    {
        public GridItemsDetail(int rowsPersPage, int totalRows, int page)
        {
            RowsPersPage = rowsPersPage;
            TotalRows = totalRows;
            Page = page;
        }

        public int TotalPages { get { return (int)Math.Ceiling((double)TotalRows / RowsPersPage); } }
        public int RowsPersPage { get; set; }
        public int TotalRows { get; set; }
        public int Page { get; set; }
        
    }
}