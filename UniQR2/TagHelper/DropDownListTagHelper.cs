using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniQR2.Flag
{
    [HtmlTargetElement("drop-down-list", Attributes = ForAttributeName + "," + ItemsAttributeName)]
    public class DropDownListTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private const string ItemsAttributeName = "asp-items";

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }


        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [HtmlAttributeName(ItemsAttributeName)]
        public IEnumerable<SelectListItem> Items { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (this.ViewContext == null || this.For == null || this.Items == null)
                return;

            output.SuppressOutput();
            output.Content.Clear();
            output.Content.AppendHtml("[Drop down list will be here!]");
        }

    }
}
