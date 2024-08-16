﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers
{
    [ExcludeFromCodeCoverage]
    [HtmlTargetElement("div", Attributes = HighlightErrorForAttributeName + "," + ErrorCssClass)]
    [HtmlTargetElement("textarea", Attributes = HighlightErrorForAttributeName + "," + ErrorCssClass)]
    [HtmlTargetElement("input", Attributes = HighlightErrorForAttributeName + "," + ErrorCssClass)]
    [HtmlTargetElement("dt", Attributes = HighlightErrorForAttributeName + "," + ErrorCssClass)]
    public class DasHighlightErrorsTagHelper : TagHelper
    {
        private const string HighlightErrorForAttributeName = "das-highlight-error-for";
        private const string ErrorCssClass = "error-class";

        [HtmlAttributeName(HighlightErrorForAttributeName)]
        public ModelExpression Property { get; set; }

        [HtmlAttributeName(ErrorCssClass)]
        public string CssClass { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        public DasHighlightErrorsTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var tagBuilder = Generator.GenerateValidationMessage(
                ViewContext,
                Property.ModelExplorer,
                Property.Name,
                message: string.Empty,
                tag: null,
                htmlAttributes: null);

            if (tagBuilder.InnerHtml.IsNullOrEmpty())
                return;

            output.AddClass(CssClass, HtmlEncoder.Default);
        }
    }
}