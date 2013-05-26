using System;
using System.Linq.Expressions;
using CC.Core.Utilities;
using CC.UI.Helpers.Configuration;
using HtmlTags;

namespace CC.UI.Helpers.Tags
{
    public interface ITagGenerator<T> where T : class
    {
        string ElementPrefix { get; set; }

        T Model { get; set; }

        HtmlTag LabelFor(Expression<Func<T, object>> expression);

        HtmlTag InputFor(Expression<Func<T, object>> expression);

        HtmlTag DisplayFor(Expression<Func<T, object>> expression);

        ElementRequest GetRequest(Expression<Func<T, object>> expression);

        HtmlTag LabelFor(ElementRequest request);

        HtmlTag InputFor(ElementRequest request);

        HtmlTag DisplayFor(ElementRequest request);

        ElementRequest GetRequest<TProperty>(Expression<Func<T, TProperty>> expression);

        ElementRequest GetRequest(Accessor accessor);
    }
}