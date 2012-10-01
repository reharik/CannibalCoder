using System.Collections.Generic;
using CC.Core.Html.Grid;

namespace CC.Core.CoreViewModelAndDTOs
{
    public class ViewModel
    {
        public int EntityId { get; set; }
        public int ParentId { get; set; }
        public int RootId { get; set; }
        public string _Title { get; set; }
        public string addUpdateUrl { get; set; }
        public string _saveUrl { get; set; }
        public string DateCreated { get; set; }
    }

    public class ListViewModel :ViewModel
    {
        public ListViewModel()
        {
            headerButtons = new List<string>();
        }

        public string deleteMultipleUrl { get; set; }
        public GridDefinition gridDef { get; set; }
        public List<string> headerButtons { get; set; }

        public string searchField { get; set; }
    }
}