using System;
using System.Collections.Generic;
using CC.Core.Html.Grid;

namespace CC.Core.CoreViewModelAndDTOs
{
    public class ViewModel
    {
        public Guid EntityId { get; set; }
        public Guid ParentId { get; set; }
        public Guid RootId { get; set; }
        public Guid Ticket { get; set; }
        public bool isNew { get; set; }
        public string _addEditUrl { get; set; }
        public string _saveUrl { get; set; }
        public string _returnUrl { get; set; }
    }

    public class ListViewModel :ViewModel
    {
        public GridDefinition gridDef { get; set; }
    }

}