using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Class representing the response of Get Affiliation call
/// </summary>
namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{
    /// <summary>
    /// Represents the Affiliation Response object
    /// </summary>
    public class ErpAffiliationResponse
    {
        /// <summary>
        /// Initializes a new instance of ErpAffiliationRespnse class
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        public ErpAffiliationResponse(bool status, string message, List<ErpAffiliation> result)
        {
            this.Status = status;
            this.Message = message;
            this.Result = result;
        }

        /// <summary>
        /// Status of the Affiliation Request Call
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Message of the Affiliation Request Call
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Result containing the List of Affiliations
        /// </summary>
        public List<ErpAffiliation> Result { get; set; }
    }
}
