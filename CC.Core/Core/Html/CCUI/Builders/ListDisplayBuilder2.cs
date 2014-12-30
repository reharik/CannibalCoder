using System.Collections.Generic;
using CC.Core.Core.Html.CCUI.HtmlConventionRegistries;
using CC.Core.HtmlTags;
using CC.Core.UI.Helpers.Configuration;

namespace CC.Core.Core.Html.CCUI.Builders
{
    public class ListDisplayBuilder2 : ElementBuilder
    {
        protected override bool matches(AccessorDef def)
        {
            return def.Accessor.PropertyType == typeof (IEnumerable<string>);
        }

        public override HtmlTag Build(ElementRequest request)
        {
            HtmlTag root = new HtmlTag("div").Attr("data-bind", "foreach: "+ CCHtmlConventionsKO.DeriveElementName(request));
            var child = new HtmlTag("div").Attr("data-bind", "text: $data" );
            root.Append(child);
            return root;
        }
    }
}