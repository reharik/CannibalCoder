using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using CC.Core.Utilities;

namespace CC.Core.Services
{
    public interface ISelectListItemService
    {
        IEnumerable<SelectListItem> CreateList<ENTITY>(IEnumerable<ENTITY> entityList,
                                                       Expression<Func<ENTITY, object>> text,
                                                       Expression<Func<ENTITY, object>> value, bool addSelectItem);

      
        IEnumerable<SelectListItem> SetSelectedItemByValue(IEnumerable<SelectListItem> entityList,
                                                           string value);

        string SetModelValueBySelected(IEnumerable<SelectListItem> list, string value);
    }

    public class SelectListItemService : ISelectListItemService
    {

    
        public IEnumerable<SelectListItem> CreateList<ENTITY>(IEnumerable<ENTITY> entityList,
                                                              Expression<Func<ENTITY, object>> text,
                                                              Expression<Func<ENTITY, object>> value, bool addSelectItem)
        {

            IList<SelectListItem> items = new List<SelectListItem>();
            Accessor textAccessor = text.ToAccessor();
            Accessor valueAccessor = value.ToAccessor();
            if (addSelectItem)
            {
                items.Add(new SelectListItem { Text = "Select Item", Value = "" });
            }
            entityList.ForEachItem(x =>
            {
                if (textAccessor.GetValue(x) != null && valueAccessor.GetValue(x) != null)
                {
                    items.Add(new SelectListItem
                    {
                        Text = textAccessor.GetValue(x).ToString(),
                        Value = valueAccessor.GetValue(x).ToString()
                    });
                }
            });
            return items.OrderBy(x => x.Text);
        }

        public string SetModelValueBySelected(IEnumerable<SelectListItem> list, string value)
        {
            return (value.IsEmpty() && list.Any(x => x.Selected)) ? list.First(x => x.Selected).Value : value;
        }

        public IEnumerable<SelectListItem> SetSelectedItemByValue(IEnumerable<SelectListItem> entityList, string value)
        {
            SelectListItem selectListItem = entityList.FirstOrDefault(x => x.Value == value);
            if (selectListItem != null)
            {
                selectListItem.Selected = true;
            }
            return entityList;
        }

    }
}
