using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using CC.Core.Localization;
using CC.Core.Services;
using CC.Core.Utilities;
using CC.Security;
using CC.Security.Interfaces;

namespace CC.Core.Html.Grid
{
    public interface IGridBuilder<ENTITY> where ENTITY : IGridEnabledClass
    {
        List<IGridColumn> columns { get; }
        IList<IDictionary<string, string>> ToGridColumns(IUser user);
        string[] ToGridRow(ENTITY item, IUser user, IEnumerable<Action<IGridColumn, ENTITY>> modifications, string gridName = "");
        string SearchField { get; set; }
        DisplayColumn<ENTITY> DisplayFor(Expression<Func<ENTITY, object>> expression);
        HiddenColumn<ENTITY> HideColumnFor(Expression<Func<ENTITY, object>> expression);
        ImageColumn<ENTITY> ImageColumn();
        ImageButtonColumn<ENTITY> ImageButtonColumn();
        LinkColumn<ENTITY> LinkColumnFor(Expression<Func<ENTITY, object>> expression, string gridName = "");
        GroupingColumn<ENTITY> GroupingColumnFor(Expression<Func<ENTITY, object>> expression);
        void SetSearchField(Expression<Func<ENTITY, object>> func);
    }

    public class GridBuilder<ENTITY> : IGridBuilder<ENTITY> where ENTITY : IGridEnabledClass
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IInjectableSiteConfig _config;

        public string SearchField { get; set; }

        public GridBuilder(IAuthorizationService authorizationService, IInjectableSiteConfig config)
        {
            _authorizationService = authorizationService;
            _config = config;
        }

        private List<IGridColumn> _columns = new List<IGridColumn>();
        public List<IGridColumn> columns
        {
            get { return _columns; }
        }

        public string[] ToGridRow(ENTITY item, IUser user, IEnumerable<Action<IGridColumn, ENTITY>> modifications, string gridName = "")
        {
            var cellValues = new List<string>();
            foreach (var column in columns)
            {
                bool isAllowed = !column.Operation.IsNotEmpty() || _authorizationService.IsAllowed(user, column.Operation);
                if (isAllowed)
                {
                    modifications.ForEachItem(x => x.Invoke(column, item));
                    string value = column.BuildColumn(item, gridName);
                    cellValues.Add(value ?? string.Empty);
                }
            }
            return cellValues.ToArray();
        }


        public IList<IDictionary<string, string>> ToGridColumns(IUser user)
        {
            var values = new List<IDictionary<string, string>>();
            foreach (var column in columns)
            {
                bool isAllowed = !column.Operation.IsNotEmpty() || _authorizationService.IsAllowed(user, column.Operation);
                if (!isAllowed) continue;
                values.Add(column.Properties);
            }
            return values;
        }

        public DisplayColumn<ENTITY> DisplayFor(Expression<Func<ENTITY, object>> expression)
        {
            return AddColumn(new DisplayColumn<ENTITY>(expression));
        }

        public HiddenColumn<ENTITY> HideColumnFor(Expression<Func<ENTITY, object>> expression)
        {
            return AddColumn(new HiddenColumn<ENTITY>(expression));
        }

        public ImageColumn<ENTITY> ImageColumn()
        {
            return AddColumn(new ImageColumn<ENTITY>(_config.Settings()));
        }

        public ImageButtonColumn<ENTITY> ImageButtonColumn()
        {
            return AddColumn(new ImageButtonColumn<ENTITY>(_config.Settings()));
        }

        public LinkColumn<ENTITY> LinkColumnFor(Expression<Func<ENTITY, object>> expression, string gridName = "")
        {
            return AddColumn(new LinkColumn<ENTITY>(expression, _config.Settings(), gridName));
        }

        public GroupingColumn<ENTITY> GroupingColumnFor(Expression<Func<ENTITY, object>> expression)
        {
            return AddColumn(new GroupingColumn<ENTITY>(expression));
        }

        public void SetSearchField(Expression<Func<ENTITY, object>> expression)
        {
            var name = LocalizationManager.GetLocalString(expression);
            SearchField = name;
        }

        public COLUMN AddColumn<COLUMN>(COLUMN column) where COLUMN : ColumnBase<ENTITY>
        {
            var count = _columns.Count;
            column.ColumnIndex = count + 1;
            _columns.Add(column);
            return column;
        }
    }

    public enum GridColumnProperties
    {
        name,
        header,
        actionUrl,
        action,
        formatter,
        formatoptions,
        sortable,
        sortColumn,
        width,
        imageName,
        hidden,
        align,
        toolTip,
        isImage,
        isClickable,
        cssClass
    }

    public enum GridColumnFormatterOptions
    {
        Number_thousandsSeparator,
        Number_decimalSeparator,
        Number_decimalPlaces,
        Number_defaultValue,
        Currency_prefix,
        Currency_suffix,
        Date_srcformat,
        Date_newformat,
    }

    public enum GridColumnAlign
    {
        Left,
        Center,
        Right,
    }

    public enum GridColumnFormatter
    {
        Integer,
        Number,
        EMail,
        Date,
        Currency,
        Checkbox,
        Time
    }

    public class GridDefinition
    {
        public string Url { get; set; }
        public string GridName { get; set; }
        public IList<IDictionary<string, string>> Columns { get; set; }
        public string Title { get; set; }
        public string SearchField { get; set; }
        
    }
}