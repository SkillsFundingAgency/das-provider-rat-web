﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Helpers
{
    [ExcludeFromCodeCoverage]
    [HtmlTargetElement("p", Attributes = ValidationForAttributeName)]
    public class DasValidationMessageTagHelper : TagHelper
    {
        private const string ValidationForAttributeName = "das-validation-for";

        [HtmlAttributeName(ValidationForAttributeName)]
        public ModelExpression Property { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        protected IHtmlGenerator Generator { get; }

        public DasValidationMessageTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (!ViewContext.ModelState.ContainsKey(Property.Name)) return;

            var tagBuilder = Generator.GenerateValidationMessage(
                ViewContext,
                Property.ModelExplorer,
                Property.Name,
                message: string.Empty,
                tag: null,
                htmlAttributes: null);

            if (tagBuilder.InnerHtml.IsNullOrEmpty())
                return;

            output.Attributes.Add("id", $"{Property.Name}-error");

            output.Content.AppendHtml(tagBuilder.InnerHtml);
        }
    }
}