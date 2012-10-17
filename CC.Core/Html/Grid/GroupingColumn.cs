using System;
using System.Linq.Expressions;
using CC.Core.Localization;
using CC.Core.Utilities;

namespace CC.Core.Html.Grid
{
    public class GroupingColumn<ENTITY> : ColumnBase<ENTITY> where ENTITY : IGridEnabledClass 
    {
        private string _groupingColumnName;

        public GroupingColumn(Expression<Func<ENTITY, object>> expression)
        {
            propertyAccessor = ReflectionHelper.GetAccessor(expression);
            Properties[GridColumnProperties.name.ToString()] = ReflectionHelper.GetProperty(expression).Name.ToSeperateWordsFromPascalCase();
            Properties[GridColumnProperties.header.ToString()] = ReflectionHelper.GetProperty(expression).Name.ToSeperateWordsFromPascalCase();
        }

        public GroupingColumn<ENTITY> FormatValue(GridColumnFormatter formatter)
        {
            Properties[GridColumnProperties.formatter.ToString()] = formatter.ToString().ToLowerInvariant();
            return this;
        }

        public GroupingColumn<ENTITY> GroupingColumnName(string groupingColumnName)
        {
            Properties[GridColumnProperties.name.ToString()] = groupingColumnName;
            return this;
        }

    }
}