
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCWebApp.Helpers
{
    public static class CustomHelpers
    {
        public static HtmlString File(this IHtmlHelper helper, string id)
        {
            
            System.Web.Mvc.TagBuilder tb = new System.Web.Mvc.TagBuilder("input");
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("id", id);
            return new HtmlString(tb.ToString());
        }

        public static HtmlString File(string id)
        {
            System.Web.Mvc.TagBuilder tb = new System.Web.Mvc.TagBuilder("input");
            tb.Attributes.Add("type", "file");
            tb.Attributes.Add("id", id);
            return new HtmlString(tb.ToString());
        }

        public static HtmlString BoldBreak(this IHtmlHelper htmlHelper, string text)
        {
            return new HtmlString($"<br/><strong>{text}</strong><br/><br/>");
        }

    }
}
