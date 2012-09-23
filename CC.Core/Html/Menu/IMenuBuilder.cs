using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CC.Core.Enumerations;
using CC.Core.Localization;
using CC.Security;
using CC.Security.Interfaces;

namespace CC.Core.Html.Menu
{
    public interface IMenuBuilder
    {
        IList<MenuItem> MenuTree(IUser user = null);
        IMenuBuilder HasChildren();
        IMenuBuilder EndChildren();
        IMenuBuilder CreateNode<CONTROLLER>(string urlPreface, Expression<Func<CONTROLLER, object>> action, StringToken text, AreaName areaName = null, string cssClass = null) where CONTROLLER : Controller;
        IMenuBuilder CreateNode<CONTROLLER>(Expression<Func<CONTROLLER, object>> action, StringToken text, AreaName areaName = null, string cssClass = null) where CONTROLLER : Controller;
        IMenuBuilder CreateNode<CONTROLLER>(Expression<Func<CONTROLLER, object>> action, StringToken text, string urlParam, AreaName areaName = null, string cssClass=null) where CONTROLLER : Controller;
        IMenuBuilder CreateNode(StringToken text, string cssClass=null);
        string OutputFlatJson();
        IMenuBuilder CreateTagNode<CONTROLLER>(StringToken text) where CONTROLLER : Controller;
        IMenuBuilder Route(string route);
    }

    public class MenuBuilder : IMenuBuilder
    {
        private readonly IAuthorizationService _authorizationService;

        public MenuBuilder(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        private IList<MenuItem> _items = new List<MenuItem>();
        private IList<MenuItem> _parentItems = new List<MenuItem>();
        public IMenuBuilder HasChildren()
        {
            var _itemList = getList();
            var lastItem = _itemList.LastOrDefault();
            lastItem.Children = new List<MenuItem>();
            _parentItems.Add(lastItem);
            return this;
        }

        public IMenuBuilder EndChildren()
        {
            var lastItem = _parentItems.LastOrDefault();
            _parentItems.Remove(lastItem);
            return this;
        }

        public IMenuBuilder CreateNode<CONTROLLER>(string urlPreface, Expression<Func<CONTROLLER, object>> action, StringToken text, AreaName areaName = null, string cssClass = null) where CONTROLLER : Controller
        {
            
            var _itemList = getList();
            _itemList.Add(new MenuItem
            {
                Text = text.DefaultValue,
                Url = urlPreface + UrlContext.GetUrlForAction(action, areaName),
                CssClass = cssClass
            });
            return this;
        }

        public IMenuBuilder CreateNode(StringToken text, string cssClass=null)
        {
            var _itemList = getList();
            _itemList.Add(new MenuItem
            {
                Text = text.DefaultValue,
                Url = "#",
                CssClass = cssClass,
                
            });
            return this;
        }

        public IMenuBuilder CreateTagNode<CONTROLLER>(StringToken text) where CONTROLLER : Controller
        {
            var _itemList = getList();
            var type = typeof(CONTROLLER).Name.ToLowerInvariant();
            type = type.Replace("controller", "");
            _itemList.Add(new MenuItem
            {
                Text = text.DefaultValue,
                Url = type
            });
            return this;
        }

        private IList<MenuItem> getList()
        {
            var lastParentItem = _parentItems.LastOrDefault();
            return lastParentItem != null ? lastParentItem.Children : _items;
        }

        public IMenuBuilder CreateNode<CONTROLLER>(Expression<Func<CONTROLLER, object>> action, StringToken text, AreaName areaName = null, string cssClass=null) where CONTROLLER : Controller
        {
            return CreateNode(action, text, "",areaName,cssClass);
        }

        public IMenuBuilder CreateNode<CONTROLLER>(Expression<Func<CONTROLLER, object>> action, StringToken text, string urlParam, AreaName areaName = null, string cssClass = null) where CONTROLLER : Controller
        {
            string param;
            if (urlParam.Contains("="))
            {
                param = urlParam.IsNotEmpty() ? "?" + urlParam : "";
            }
            else
            {
                param = urlParam.IsNotEmpty() ? "/" + urlParam : "";
            }

            var _itemList = getList();
            _itemList.Add(new MenuItem
            {
                Text = text.DefaultValue,
                Url = UrlContext.GetUrlForAction(action,areaName) + param,
                CssClass=cssClass
            });
            return this;
        }
        public IMenuBuilder Route(string route)
        {
            var _itemList = getList();
            var lastItem = _itemList.LastOrDefault();
            lastItem.Url=route;
            return this;

        } 
        public IList<MenuItem> MenuTree(IUser user = null)
        {
            if (user == null) return _items;
            IList<MenuItem> permittedItems = modifyListForPermissions(user);
            return permittedItems;
        }

        private IList<MenuItem> modifyListForPermissions(IUser user)
        {
            var permittedItems = new List<MenuItem>();
            _items.ForEachItem(x =>
            {
                var operationName = "/MenuItem/" + x.Text.RemoveWhiteSpace();
                if (_authorizationService.IsAllowed(user, operationName))
                {
                    permittedItems.Add(x);
                }
            });
            return permittedItems;
        }

        private void getLinksOnly(IEnumerable<MenuItem> items, IList<MenuItem> result)
        {
            foreach (var x in items)
            {
                if(x.Text.IsEmpty())
                {
                    continue;
                }
                if(x.Children==null)
                {
                    result.Add(x);
                    continue;
                }
                getLinksOnly(x.Children, result);
            }
        }

        public string OutputFlatJson()
        {
            var result = new List<MenuItem>();
            getLinksOnly(_items,result);
            result.ForEachItem(x=>Debug.WriteLine(x.Text + "--" +x.Url));
            var jss = new JavaScriptSerializer();
            var json = jss.Serialize(result);
            return json;
        }
    }

    public class MenuItem
    {
        public string Text { get; set; }
        public string Url { get; set; }
        public string CssClass { get; set; }
        public IList<MenuItem> Children { get; set; }
    }
}