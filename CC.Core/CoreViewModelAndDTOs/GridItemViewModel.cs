using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Mvc;
using CC.Core.Html.Grid;

namespace CC.Core.CoreViewModelAndDTOs
{
    public class GridItemsViewModel
    {
        public int total { get; set; }
        public int page { get; set; }
        public int records { get; set; }
        public IEnumerable items { get; set; }
        public IDictionary<string, string> userdata { get; set; }

        public int GetTotal(int totalRecordsAvailable)
        {
            if (records <= 0)
                return 0;
            decimal subTotal = (decimal)records / (decimal)totalRecordsAvailable;
            return subTotal < 1 ? 1 : (int)Math.Ceiling(subTotal);
        }
    }

    [ModelBinder(typeof(GridModelBinder))]
    public class GridItemsRequestModel
    {
        public bool IsSearch { get; set; }
        public int page { get; set; }
        public int rows { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
        public Filter filter { get; set; }
    }

    [DataContract]
    public class Filter
    {
        [DataMember]
        public string groupOp { get; set; }
        [DataMember]
        public Rule[] rules { get; set; }

        public static Filter Create(string jsonData)
        {
            try
            {
                var serializer = new DataContractJsonSerializer(typeof(Filter));
                //var ms = new System.IO.MemoryStream(Encoding.Default.GetBytes(jsonData));
                var ms = new System.IO.MemoryStream(
                    Encoding.Convert(
                        Encoding.Default,
                        Encoding.UTF8,
                        Encoding.Default.GetBytes(jsonData)));
                return serializer.ReadObject(ms) as Filter;
            }
            catch
            {
                return null;
            }
        }
    }

    [DataContract]
    public class Rule
    {
        [DataMember]
        public string field { get; set; }
        [DataMember]
        public string op { get; set; }
        [DataMember]
        public string data { get; set; }
    }

    public class GridModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                var request = controllerContext.HttpContext.Request;
                return new GridItemsRequestModel
                {
                    IsSearch = bool.Parse(request["_search"] ?? "false"),
                    page = int.Parse(request["page"] ?? "1"),
                    rows = int.Parse(request["rows"] ?? "10"),
                    SortColumn = request["sidx"] ?? "",
                    SortOrder = request["sord"] ?? "asc",
                    filter = Filter.Create(request["filter"] ?? "")
                };
            }
            catch
            {
                return null;
            }
        }
    }







//    public class GridItemsRequestModel : ListViewModel
//    {
//        public string Name { get; set; }
//        public int page { get; set; }
//        public int rows { get; set; }
//        public string sidx { get; set; }
//        public string sord { get; set; }
//        public bool showDeleted { get; set; }
//        public bool showArchived { get; set; }
//        private PageSortFilter _pageSortFilter;
//        public PageSortFilter PageSortFilter
//        {
//            get
//            {
//                bool sortAscending = true;
//                if (sord != null && sord.Equals("desc", StringComparison.OrdinalIgnoreCase)) sortAscending = false;
//                return new PageSortFilter(page, rows, sidx, sortAscending);
//            }
//            set { _pageSortFilter = value; }
//        }
//
//        public string filters { get; set; }
//    }

}