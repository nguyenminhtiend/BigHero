using System.Web.Mvc;
using HvN.BigHero.DAL.Model;
using HvN.BigHero.DAL.Utility;

namespace HvN.BigHero.Web.Helper
{
    public static class HtmlExtension
    {
        public static MvcHtmlString LabelForColumn(this HtmlHelper htmlHelper, ColumnViewModel columnViewModel)
        {
            var builder = new TagBuilder("label");
            builder.MergeAttribute("for", columnViewModel.Name);
            if (!columnViewModel.NullAble)
            {
                builder.AddCssClass("mandatory");
            }
            builder.SetInnerText(columnViewModel.Display + " :");
            
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));
        }
        public static MvcHtmlString EditorForColumn(this HtmlHelper htmlHelper, ColumnViewModel columnViewModel, string css = "")
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("name", columnViewModel.Name);
            if (columnViewModel.Value != null)
            {
                builder.MergeAttribute("value", columnViewModel.Value.ToString());
            }
            string cssClass = css;
            if (!columnViewModel.NullAble)
            {
                cssClass += " mandatory";
            }
            if (columnViewModel.DataType == ColumnType.DateTime)
            {
                cssClass += " datetimepicker";
            }
            builder.AddCssClass(cssClass);
            builder.MergeAttribute("placeholder", columnViewModel.Display);
            if (columnViewModel.Size.HasValue)
            {
                builder.MergeAttribute("maxlength", columnViewModel.Size.Value.ToString());
            }
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}