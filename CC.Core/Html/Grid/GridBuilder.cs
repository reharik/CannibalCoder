using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CC.Core.Html.Grid
{
    public interface IGridBuilder<ENTITY> where ENTITY : IGridEnabledClass
    {
        List<IGridColumn> columns { get; }
        IList<IDictionary<string, string>> ToGridColumns();
        string[] ToGridRow(ENTITY item, IEnumerable<Action<IGridColumn, ENTITY>> modifications, string gridName = "");

        DisplayColumn<ENTITY> DisplayFor(Expression<Func<ENTITY, object>> expression);
        HiddenColumn<ENTITY> HideColumnFor(Expression<Func<ENTITY, object>> expression);
        ImageColumn<ENTITY> ImageColumn();
        ImageButtonColumn<ENTITY> ImageButtonColumn(string JSApplicationName);
        LinkColumn<ENTITY> LinkColumnFor(Expression<Func<ENTITY, object>> expression, string JSApplicationName, string gridName = "");
        GroupingColumn<ENTITY> GroupingColumnFor(Expression<Func<ENTITY, object>> expression);
    }

    public class GridBuilder<ENTITY> : IGridBuilder<ENTITY> where ENTITY : IGridEnabledClass
    {

        public GridBuilder()
        {
        }

        private List<IGridColumn> _columns = new List<IGridColumn>();
        public List<IGridColumn> columns
        {
            get { return _columns; }
        }

        public string[] ToGridRow(ENTITY item, IEnumerable<Action<IGridColumn, ENTITY>> modifications, string gridName = "")
        {
            var cellValues = new List<string>();
            foreach (var column in columns)
            {
               //TODO put security here
                    modifications.ForEachItem(x => x.Invoke(column, item));
                    string value = column.BuildColumn(item, gridName);
                    cellValues.Add(value ?? string.Empty);
            }
            return cellValues.ToArray();
        }

        public IList<IDictionary<string, string>> ToGridColumns()
        {
            var values = new List<IDictionary<string, string>>();
            foreach (var column in columns)
            {
                //TODO put security here
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
            return AddColumn(new ImageColumn<ENTITY>());
        }

        public ImageButtonColumn<ENTITY> ImageButtonColumn(string JSApplicationName)
        {
            return AddColumn(new ImageButtonColumn<ENTITY>(JSApplicationName));
        }

        public LinkColumn<ENTITY> LinkColumnFor(Expression<Func<ENTITY, object>> expression,string JSApplicationName, string gridName = "")
        {
            return AddColumn(new LinkColumn<ENTITY>(expression,JSApplicationName, gridName));
        }

        public GroupingColumn<ENTITY> GroupingColumnFor(Expression<Func<ENTITY, object>> expression)
        {
            return AddColumn(new GroupingColumn<ENTITY>(expression));
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
    }
}