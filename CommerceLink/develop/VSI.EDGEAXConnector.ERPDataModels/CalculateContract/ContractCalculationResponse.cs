using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class ContractCalculationResponse
    {

        /// <summary>
        /// Initializes a new instance of the CartResponse
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="errorMessage">error Message</param>
        /// <param name="contractCart">cart</param>
        public ContractCalculationResponse(bool status, string errorMessage, CLCart contractCart)
        {
            this.Status = status;
            this.ErrorMessage = errorMessage;
            this.Cart = contractCart;
        }

        /// <summary>
        /// Status of cart
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// ErrorMessage of cart
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Cart
        /// </summary>
        public CLCart Cart { get; set; }

    }
}
