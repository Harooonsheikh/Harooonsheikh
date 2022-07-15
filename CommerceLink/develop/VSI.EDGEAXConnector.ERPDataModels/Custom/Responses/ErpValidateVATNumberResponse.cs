using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom.Responses
{


    /// <summary>
    /// 
    /// </summary>
    public class ErpValidateVATNumberResponse
    {

        /// <summary>
        /// Initializes a new instance of the ValidateVATNumberResponse
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="result"></param>
        public ErpValidateVATNumberResponse(bool status, string message, string result)
        {
            this.Status = status;
            this.Message = message;
            this.Result = result;
        }

        /// <summary>
        /// status of sales order trans
        /// </summary>
        public bool Status { get; set; }


        /// <summary>
        /// message of sales order trans
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Result { get; set; }

    }


}
