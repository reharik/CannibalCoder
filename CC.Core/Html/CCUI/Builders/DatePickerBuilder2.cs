using System;
using System.Collections.Generic;
using CC.Core.CoreViewModelAndDTOs;
using CC.Core.Html.CCUI.HtmlConventionRegistries;
using CC.Core.Html.CCUI.HtmlConventionRegistries;
using CC.Core.Html.CCUI.Tags;
using CC.UI.Helpers.Configuration;
using HtmlTags;

namespace CC.Core.Html.CCUI.Builders
{
    public class TextboxBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return true;
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new TextboxTag().Attr("data-bind", "value:" + CCHtmlConventions2.DeriveElementName(request));
        }
    }

    public class DatePickerBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return (def.Accessor.PropertyType == typeof(DateTime)
                || def.Accessor.PropertyType == typeof(DateTime?))
                && !def.Accessor.FieldName.EndsWith("Time");
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new TextboxTag().Attr("data-bind", "dateString:" + CCHtmlConventions2.DeriveElementName(request)).AddClass("datePicker");
        }
    }

    public class TimePickerBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return (def.Accessor.PropertyType == typeof(DateTime)
                || def.Accessor.PropertyType == typeof(DateTime?))
                && def.Accessor.FieldName.EndsWith("Time");
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new TextboxTag().Attr("data-bind", "timeString:" + CCHtmlConventions2.DeriveElementName(request)).AddClass("timePicker");
        }
    }

    public class ImageBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.PropertyType == typeof(string)
                && def.Accessor.FieldName.EndsWith("FileUrl");
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new HtmlTag("img").Attr("data-bind",
                " attr: { src: " + CCHtmlConventions2.DeriveElementName(request) + " }")
                .Attr("alt", request.Accessor.FieldName);
        }
    }

    public class CheckboxBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.PropertyType == typeof (bool);
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new CheckboxTag(false).Attr("data-bind",
                                          "checked:" + CCHtmlConventions2.DeriveElementName(request));
        }
    }

    public class PasswordBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.Name.ToLowerInvariant().Contains("password");
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new PasswordTag().Attr("data-bind", "value:" + CCHtmlConventions2.DeriveElementName(request));
        }
    }

    public class EmailDisplayBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.FieldName.ToLowerInvariant().Contains("email");
        }

        public override HtmlTag Build(ElementRequest request)
        {
            HtmlTag root = new HtmlTag("a");
            root.Attr("data-bind", "attr: { href: mailto:" + CCHtmlConventions2.DeriveElementName(request)+"}");
            root.Children.Add(new HtmlTag("span").Attr("data-bind", "text:" + CCHtmlConventions2.DeriveElementName(request)));
            return root;
        }
    }

    public class MultiSelectBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.PropertyType.GetInterface("ITokenInputViewModel")!=null;
        }

        public override HtmlTag Build(ElementRequest request)
        {
            return new TextboxTag().Id(request.Accessor.Name).AddClass("multiSelect").Attr("data-bind", "MultiSelect:" + CCHtmlConventions2.DeriveElementName(request));
        }
    }

    public class PictureGallery : ElementBuilder
    {
        protected  override  bool matches(AccessorDef def)
        {
            return def.Accessor.PropertyType == typeof(IEnumerable<PhotoDto>);
        }
        public override HtmlTag Build(ElementRequest request)
        {
            var ul = new HtmlTag("ul").Attr("data-bind", "foreach:" + CCHtmlConventions2.DeriveElementName(request));
            var li = new HtmlTag("li");
            li.Children.Add(new HtmlTag("image").Attr("data-bind", "attr:{src:FileUrl}"));
            ul.Children.Add(li);
            return ul;
        }
    }

    public class FileUploader : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.PropertyType == typeof(string)
               && (def.Accessor.FieldName.EndsWith("FileUrl"));
        }
        public override HtmlTag Build(ElementRequest request)
        {
            var container = new HtmlTag("div").AddClass("imageInputContainer");
            var imageContainer = new HtmlTag("div").AddClass("imageContainer");
            var name = CCHtmlConventions2.DeriveElementName(request);
            var thumb = new HtmlTag("img").Attr("data-bind", "attr: { src: " + name + " }").Attr("alt", request.Accessor.FieldName);
            var delete = new HtmlTag("input").Attr("type", "button").AddClass("deleteImage").Attr("value", "     Delete");

            var inputContainer = new HtmlTag("div").AddClass("inputContainer");
            var file = new HtmlTag("input").Attr("type", "file").Attr("size", 45).Attr("id", name).Attr("name", name);
            imageContainer.Children.Add(thumb);
            imageContainer.Children.Add(delete);
            inputContainer.Children.Add(file);
            container.Children.Add(imageContainer);
            container.Children.Add(inputContainer);
            return container;
        }
    }
}

