using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <inheritdoc />
    public class HomeController : ApiController
    {
        /// <summary>
        /// Default action to run on application startup
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Success");
        }

        /// <summary>
        /// Default action to run on application startup
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/v1/Error/{requestId:guid}")]
        [Route("api/v1/Request/{requestId:guid}")]
        [Route("Request/{requestId:guid}")]
        public RequestDetailsVM GetErrorDetails(string requestId)
        {
            if (string.IsNullOrEmpty(requestId))
            {
                throw new CommerceLinkError("Error - Request ID is empty.");
            }
            else
            {
                return CommerceLinkLogger.GetErrorDetails(requestId);
            }
            
        }
    }
}
