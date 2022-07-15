using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ganss.XSS;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.DashboardApi.ActionFilters
{
    /// <summary>
    /// 
    /// </summary>
    public class SanitizeInputAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Sanitize every input on the entry point
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            SanitizeInput(actionContext);
        }
        /// <summary>
        /// This method has actual implementation for input Sanitization
        /// </summary>
        /// <param name="actionContext"></param>
        private void SanitizeInput(HttpActionContext actionContext)
        {
            var lstIgnoreApis = Convert.ToString(ConfigurationManager.AppSettings["IgnoreAPIsForSanitizing"]).Split(',');

            if (lstIgnoreApis.Any(a=> actionContext.Request.RequestUri.LocalPath.Equals(a)))
            {
                // Ignore Sanitizing for these APIs
                return;
            }

            HtmlSanitizer sanitizer = new HtmlSanitizer();

            var allowedTags = Convert.ToString(ConfigurationManager.AppSettings["AllowedHTMLTags"]);
            var allowedAttributes = Convert.ToString(ConfigurationManager.AppSettings["AllowedHTMLAttributes"]);
            sanitizer.AllowedTags.Clear();
            sanitizer.AllowedAtRules.Clear();
            sanitizer.AllowedAttributes.Clear();
            sanitizer.AllowedClasses.Clear();
            sanitizer.AllowedCssProperties.Clear();
            sanitizer.AllowedSchemes.Clear();

            if (!string.IsNullOrEmpty(allowedTags))
            {
                var lstAllowedTags = allowedTags.Split(',');
                foreach (var allowedTag in lstAllowedTags)
                {
                    sanitizer.AllowedTags.Add(allowedTag);
                }
            }

            if (!string.IsNullOrEmpty(allowedAttributes))
            {
                var lstAllowedAttributes = allowedAttributes.Split(',');
                foreach (var allowedAttr in lstAllowedAttributes)
                {
                    sanitizer.AllowedAttributes.Add(allowedAttr);
                }
            }

            sanitizer.RemovingTag += (sender, args) =>
                throw new CommerceLinkError("Invalid Tag " + args.Tag.TagName +
                                                         " found in input");
            sanitizer.RemovingAttribute += (sender, args) =>
                throw new CommerceLinkError("Invalid Attribute " + args.Attribute.Name +
                                                         " found in input");

            if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count == 1)
            {
                var requestBody = JsonConvert.SerializeObject(actionContext.ActionArguments.First());
                sanitizer.Sanitize(requestBody);
            }
        }

        #region ADDITIONAL INFORMTION.  
        // Currently our main purpose was to hande handle XSS for javascript all other setting is left defatul.Below are the few settings to white list additional elements.

        //Configure allowed HTML tags through the property AllowedTags.All other tags will be stripped.
        //Configure allowed HTML attributes through the property AllowedAttributes. All other attributes will be stripped.
        //Configure allowed CSS property names through the property AllowedCssProperties.All other styles will be stripped.
        //Configure allowed CSS at-rules through the property AllowedAtRules.All other at-rules will be stripped.
        //Configure allowed URI schemes through the property AllowedSchemes. All other URIs will be stripped.
        //Configure HTML attributes that contain URIs (such as "src", "href" etc.) through the property UriAttributes.
        //Provide a base URI that will be used to resolve relative URIs against.
        #endregion
    }
}