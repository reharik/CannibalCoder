using System;
using System.Linq.Expressions;
using CC.Core.Utilities;
using CC.UI.Helpers.Configuration;
using HtmlTags;
using Microsoft.Practices.ServiceLocation;

namespace CC.UI.Helpers.Tags
{
    public class TagGenerator<T> : ITagGenerator<T> where T : class
    {
        private readonly IElementNamingConvention _namingConvention;
        private T _model;
        private TagProfile _profile;

        public TagGenerator(IElementNamingConvention namingConvention)
        {
            ElementPrefix = string.Empty;
            _namingConvention = namingConvention;
            _profile = new TagProfile("default");
        }

        #region ITagGenerator<T> Members

        public T Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public string ElementPrefix { get; set; }

        public ElementRequest GetRequest(Expression<Func<T, object>> expression)
        {
            return GetRequest(expression.ToAccessor());
        }

        public ElementRequest GetRequest(Accessor accessor)
        {
            var request = new ElementRequest(_model, accessor);
            determineElementName(request);
            return request;
        }

        public ElementRequest GetRequest<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var request = new ElementRequest(_model, ReflectionHelper.GetAccessor(expression));
            determineElementName(request);
            return request;
        }

//        public HtmlTag DisplayFor(Expression<Func<T, object>> expression, string profile)
//        {
//            return buildTag(expression, _library[profile].Display);
//        }

        public HtmlTag LabelFor(ElementRequest request)
        {
            return _profile.Label.Build(request);
        }

        public HtmlTag InputFor(ElementRequest request)
        {
            return _profile.Editor.Build(request);
        }

        public HtmlTag DisplayFor(ElementRequest request)
        {
            return _profile.Display.Build(request);
        }

        public HtmlTag LabelFor(Expression<Func<T, object>> expression)
        {
            return buildTag(expression, _profile.Label);
        }

//        public HtmlTag LabelFor(Expression<Func<T, object>> expression, string profile)
//        {
//            return buildTag(expression, _library[profile].Label);
//        }

        public HtmlTag InputFor(Expression<Func<T, object>> expression)
        {
            return buildTag(expression, _profile.Editor);
        }

//        public HtmlTag InputFor(Expression<Func<T, object>> expression, string profile)
//        {
//            return buildTag(expression, _library[profile].Editor);
//        }

        public HtmlTag DisplayFor(Expression<Func<T, object>> expression)
        {
            return buildTag(expression, _profile.Display);
        }

        #endregion

        private HtmlTag buildTag(Expression<Func<T, object>> expression, TagFactory factory)
        {
            ElementRequest request = GetRequest(expression);
            return factory.Build(request);
        }

        private void determineElementName(ElementRequest request)
        {
            string str = ElementPrefix ?? string.Empty;
            request.ElementId = str + _namingConvention.GetName(typeof (T), request.Accessor);
        }
    }
}