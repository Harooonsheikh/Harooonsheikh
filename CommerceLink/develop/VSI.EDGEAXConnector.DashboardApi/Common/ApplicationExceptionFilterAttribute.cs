using System.Web.Http.Filters;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DashboardApi.Common
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
            CustomLogger logger = new CustomLogger(new LoggerContext { UserId = "Global", StoreId = 1 });
            logger.LogException(context.Exception);
            base.OnException(context);
        }

        #endregion

    }
}