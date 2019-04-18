using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Web.UI.Helpers
{
    public static class HtmlCustomHelper
    {
        public static MvcHtmlString CheckBoxSimpleFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes)
        {
            string checkBoxWithHidden = htmlHelper.CheckBoxFor(expression, htmlAttributes).ToHtmlString().Trim();
            string pureCheckBox = checkBoxWithHidden.Substring(0, checkBoxWithHidden.IndexOf("<input", 1));
            return new MvcHtmlString(pureCheckBox);
        }

        public static MvcHtmlString CustomTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool bReadonly = false)
        {
            var member = expression.Body as MemberExpression;
            var stringLength = member.Member
                .GetCustomAttributes(typeof(StringLengthAttribute), false)
                .FirstOrDefault() as StringLengthAttribute;

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            if (stringLength != null)
            {
                attributes.Add("maxlength", stringLength.MaximumLength);
            }
            if (bReadonly)
            {
                attributes.Add("readonly", "true");
            }
            return htmlHelper.TextBoxFor(expression, attributes);
        }

        public static MvcHtmlString CustomTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string format, object htmlAttributes, bool bReadonly = false)
        {
            var member = expression.Body as MemberExpression;
            var stringLength = member.Member
                .GetCustomAttributes(typeof(StringLengthAttribute), false)
                .FirstOrDefault() as StringLengthAttribute;

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            if (stringLength != null)
            {
                attributes.Add("maxlength", stringLength.MaximumLength);
            }
            if (bReadonly)
            {
                attributes.Add("readonly", "true");
            }
            return htmlHelper.TextBoxFor(expression, format, attributes);
        }

        public static MvcHtmlString CustomTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool bReadonly = false)
        {
            var member = expression.Body as MemberExpression;
            var stringLength = member.Member
                .GetCustomAttributes(typeof(StringLengthAttribute), false)
                .FirstOrDefault() as StringLengthAttribute;

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            if (stringLength != null)
            {
                attributes.Add("maxlength", stringLength.MaximumLength);
            }
            if (bReadonly)
            {
                attributes.Add("readonly", "true");
            }
            return htmlHelper.TextAreaFor(expression, attributes);
        }
    }
}