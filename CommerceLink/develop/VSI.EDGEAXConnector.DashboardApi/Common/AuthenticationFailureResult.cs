using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VSI.EDGEAXConnector.DashboardApi.Common
{

    /// <summary>
    /// AuthenticationFailureResult Creates an System.Net.Http.HttpResponseMessage asynchronously.
    /// </summary>
    public class AuthenticationFailureResult : IHttpActionResult
    {

        #region Properties

        /// <summary>
        /// This property used for reason phrase
        /// </summary>
        public string ReasonPhrase { get; private set; }

        /// <summary>
        /// It used for System.Net.Http.HttpRequestMessage asynchronously.
        /// </summary>
        public HttpRequestMessage Request { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="reasonPhrase"></param>
        /// <param name="request"></param>
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// ExecuteAsync executes task.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function returns HttpResponseMessage object.
        /// </summary>
        /// <returns></returns>
        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }

        #endregion
    }
}