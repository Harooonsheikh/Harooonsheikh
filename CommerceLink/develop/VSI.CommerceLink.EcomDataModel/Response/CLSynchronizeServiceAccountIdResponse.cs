using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel.Response
{
    public class CLSynchronizeServiceAccountIdResponse
    {
        /// <summary>
        /// Initializes a new instance of the CLSynchronizeServiceAccountIdResponse
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="errorMessage">error Message</param>
        public CLSynchronizeServiceAccountIdResponse(bool status, string errorMessage)
        {
            this.Status = status;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Status of request
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// ErrorMessage for request
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
