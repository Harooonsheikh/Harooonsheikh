using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.CommerceLink.EcomDataModel.Response
{
    public class CLContactPersonResponse
    {            
        /// <summary>
        /// Initializes a new instance of the ContactPersonResponse
        /// </summary>
        /// <param name="status">status</param>
        /// <param name="errorMessage">error Message</param>
        /// <param name="contactPerson">contact person</param>
        public CLContactPersonResponse(bool status, string errorMessage, ErpContactPerson contactPerson)
        {
            this.Status = status;
            this.ErrorMessage = errorMessage;
            this.ContactPerson = contactPerson;
        }

        /// <summary>
        /// Status of contact person
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// ErrorMessage of contact person
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// ContactPerson of contact person
        /// </summary>
        public ErpContactPerson ContactPerson { get; set; }
    }
}
