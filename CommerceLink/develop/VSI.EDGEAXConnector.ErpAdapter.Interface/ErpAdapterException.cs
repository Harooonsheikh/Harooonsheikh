using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public sealed class ErpAdapterException : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErpAdapterException"/> class.
        /// </summary>
        public ErpAdapterException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErpAdapterException"/> class.
        /// </summary>
        /// <param name="errorResourceId"></param>
        public ErpAdapterException(string errorResourceId) :
            base("Error Resource ID: " + errorResourceId)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErpAdapterException"/> class.
        /// </summary>
        /// <param name="errorResourceId"></param>
        /// <param name="message"></param>
        public ErpAdapterException(string errorResourceId, string message) :
            base("Error Resource ID: " + errorResourceId + ", message: " + message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ErpAdapterException"/> class.
        /// </summary>
        /// <param name="errorResourceId"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ErpAdapterException(string errorResourceId, string message, Exception innerException) :
            base("Error Resource ID: " + errorResourceId + ", message: " + message, innerException)
        {
        }
    }
}
