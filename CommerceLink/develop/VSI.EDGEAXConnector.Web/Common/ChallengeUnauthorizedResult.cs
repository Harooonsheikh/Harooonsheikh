using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VSI.EDGEAXConnector.Web.Common
{

    /// <summary>
    /// ChallengeUnauthorizedResult used for Http result.
    /// </summary>
    public class ChallengeUnauthorizedResult : IHttpActionResult
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="challenge"></param>
        /// <param name="innerResult"></param>
        public ChallengeUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        #endregion

        #region Properties

        /// <summary>
        /// It used to authenticate header value.
        /// </summary>
        public AuthenticationHeaderValue Challenge { get; private set; }

        /// <summary>
        /// It used for HttpResponseMessage asynchronously.
        /// </summary>
        public IHttpActionResult InnerResult { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// ExecuteAsync Creates an System.Net.Http.HttpResponseMessage asynchronously.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }

        #endregion

    }
}