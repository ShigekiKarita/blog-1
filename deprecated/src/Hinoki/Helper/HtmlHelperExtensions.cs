using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hinoki.Core;
using Hinoki.Models;

namespace Hinoki.Helper
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString ArticleTags(this HtmlHelper helper, List<HNKTag> items, string prefixString)
        {
            if (items == null || items.Count < 1)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            TagBuilder spanTag = new TagBuilder("span");
            spanTag.MergeAttribute("class", "entry-header-meta entry-tag");

            StringBuilder innerHtml = new StringBuilder();
            innerHtml.Append(prefixString);

            int idx = 0;
            foreach (HNKTag item in items)
            {
                TagBuilder aTag = new TagBuilder("a");
                // UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                // string url = urlHelper.Action("Index", "Article", new { id = item.Id });
                // aTag.MergeAttribute("href", url);
                aTag.InnerHtml = item.Name;
                innerHtml.Append(aTag.ToString());

                if (idx + 1 != items.Count)
                {
                    innerHtml.Append(", ");
                }
                idx++;
            }
            spanTag.InnerHtml = innerHtml.ToString();

            return MvcHtmlString.Create(spanTag.ToString());
        }
    }
}