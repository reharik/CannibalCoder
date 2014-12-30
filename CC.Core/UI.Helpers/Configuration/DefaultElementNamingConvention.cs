using System;
using CC.Core.Utilities;

namespace CC.Core.UI.Helpers.Configuration
{
    public class DefaultElementNamingConvention : IElementNamingConvention
    {
        #region IElementNamingConvention Members

        public string GetName(Type modelType, Accessor accessor)
        {
            return accessor.Name;
        }

        #endregion
    }
}