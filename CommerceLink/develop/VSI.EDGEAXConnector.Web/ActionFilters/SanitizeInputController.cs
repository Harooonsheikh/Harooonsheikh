using System.Linq;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Ganss.XSS;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Web.ActionFilters
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
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            sanitizer.RemovingTag += Sanitizer_RemovingTag;

            if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count == 1)
            {
                var requestBody = JsonConvert.SerializeObject(actionContext.ActionArguments.First());
                sanitizer.Sanitize(requestBody);
            }
        }

        private void Sanitizer_RemovingTag(object sender, RemovingTagEventArgs e)
        {
            throw new CommerceLinkError("Inalid tag " + e.Tag.TagName + " found in input");
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
