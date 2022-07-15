using System.Web.Http.Filters;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Web.Common
{

    /// <summary>
    /// ApplicationExceptionFilterAttribute class privides the attributes for the exception filter.
    /// </summary>
    public class ApplicationExceptionFilterAttribute : ExceptionFilterAttribute
    {

        #region Public Methods

        /// <summary>
        /// OnException raises the exception event.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(HttpActionExecutedContext context)
        {
            CustomLogger.LogException(context.Exception, 1 , "Global"); //TODO
            base.OnException(context);
        }

        #endregion

    }
}