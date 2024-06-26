﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SFA.DAS.ProviderRequestApprenticeTraining.Web.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ZenDeskLabelExtensions
    {
        public static HtmlString SetZenDeskLabels(this IHtmlHelper html, params string[] labels)
        {
            var keywords = string.Join(",", labels
                .Where(label => !string.IsNullOrEmpty(label))
                .Select(label => $"'{EscapeApostrophes(label)}'"));

            // when there are no keywords default to empty string to prevent zen desk matching articles from the url
            var apiCallString = "<script type=\"text/javascript\">zE('webWidget', 'helpCenter:setSuggestions', { labels: ["
                                + (!string.IsNullOrEmpty(keywords) ? keywords : "''")
                                + "] });</script>";

            return new HtmlString(apiCallString);
        }

        private static string EscapeApostrophes(string input)
        {
            return input.Replace("'", @"\'");
        }
    }
}