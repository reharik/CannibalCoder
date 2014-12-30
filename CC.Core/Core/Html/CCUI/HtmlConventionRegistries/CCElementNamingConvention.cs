using System;
using CC.Core.UI.Helpers.Configuration;
using CC.Core.Utilities;

namespace CC.Core.Core.Html.CCUI.HtmlConventionRegistries
{
    public class CCElementNamingConvention : IElementNamingConvention
    {
        public string GetName(Type modelType, Accessor accessor)
        {
            return accessor.FieldName;
        }
    }
}