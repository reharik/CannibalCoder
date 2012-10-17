using System;
using System.Linq.Expressions;
using CC.Core.Localization;
using CC.Core.Utilities;

namespace CC.Core.Html.Grid
{
    public class HiddenColumn<ENTITY> : ColumnBase<ENTITY> where ENTITY : IGridEnabledClass 
    {
        public HiddenColumn(Expression<Func<ENTITY, object>> expression)
        {
            propertyAccessor = ReflectionHelper.GetAccessor(expression);
            Properties[GridColumnProperties.hidden.ToString()] = "true";
            Properties[GridColumnProperties.name.ToString()] = ReflectionHelper.GetProperty(expression).Name.ToSeperateWordsFromPascalCase();
            Properties[GridColumnProperties.header.ToString()] = ReflectionHelper.GetProperty(expression).Name.ToSeperateWordsFromPascalCase();
        }
    }
}