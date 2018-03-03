using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Hinoki.Areas.Admin.Models;

namespace Hinoki.Areas.Admin.Helper
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlString Breadcrumbs(this HtmlHelper helper, List<BreadcrumbItem> items)
        {
            if (items == null || items.Count < 1)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            TagBuilder olTag = new TagBuilder("ol");
            olTag.MergeAttribute("class", "breadcrumb");

            StringBuilder innerHtml = new StringBuilder();
            foreach (BreadcrumbItem item in items)
            {
                TagBuilder liTag = new TagBuilder("li");
                TagBuilder spanTag = new TagBuilder("span");
                TagBuilder aTag = new TagBuilder("a");
                TagBuilder span2Tag = new TagBuilder("span");

                spanTag.MergeAttribute("itemscope", string.Empty);
                spanTag.MergeAttribute("itemtype", "http://data-vocabulary.org/Breadcrumb");
                aTag.MergeAttribute("href", item.Url);
                aTag.MergeAttribute("itemprop", "url");
                span2Tag.MergeAttribute("itemprop", "title");
                span2Tag.SetInnerText(item.Title);

                aTag.InnerHtml = span2Tag.ToString();
                spanTag.InnerHtml = aTag.ToString();
                liTag.InnerHtml = spanTag.ToString();

                innerHtml.Append(liTag.ToString());
            }
            olTag.InnerHtml = innerHtml.ToString();

            return MvcHtmlString.Create(olTag.ToString());
        }

        /// <summary>
        /// ページング
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static IHtmlString Pagination(this HtmlHelper helper, int totalCount, int pageSize, int pageNumber)
        {
            if (totalCount < 1 || pageSize < 1 || pageNumber < 1)
            {
                return MvcHtmlString.Create(string.Empty);
            }

            int hoge = (int)Math.Ceiling(((double)totalCount / (double)pageSize));

            TagBuilder ulTag = new TagBuilder("ul");
            ulTag.MergeAttribute("class", "pagination");

            StringBuilder innerHtml = new StringBuilder();
            for (int i = 0; i < hoge; i++)
            {
                int nextPageNumber = i + 1;

                TagBuilder liTag = new TagBuilder("li");
                if (nextPageNumber == pageNumber)
                {
                    liTag.MergeAttribute("class", "active");
                }

                TagBuilder aTag = new TagBuilder("a");

                UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
                string url = urlHelper.Action("Index", "Article", new { p = nextPageNumber, l = pageSize });
                aTag.MergeAttribute("href", url);
                aTag.InnerHtml = nextPageNumber.ToString();
                liTag.InnerHtml = aTag.ToString();

                innerHtml.Append(liTag.ToString());
            }
            ulTag.InnerHtml = innerHtml.ToString();

            return MvcHtmlString.Create(ulTag.ToString());
        }
    }
}