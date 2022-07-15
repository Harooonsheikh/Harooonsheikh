namespace VSI.EDGEAXConnector.CRT.Interface
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    //using Microsoft.Dynamics.Commerce.Runtime;

    /// <summary>
    /// Represents the AX Adapter related exceptions generated from proxy.
    /// </summary>
    public sealed class AXAdapterException : System.Exception
    {
        private string resourceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AXAdapterException"/> class.
        /// </summary>
        public AXAdapterException() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AXAdapterException"/> class.
        /// </summary>
        /// <param name="errorResourceId"></param>
        public AXAdapterException(string errorResourceId) : 
            base("Error Resource ID: " + errorResourceId)
        {
            this.resourceId = errorResourceId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AXAdapterException"/> class.
        /// </summary>
        /// <param name="errorResourceId"></param>
        /// <param name="message"></param>
        public AXAdapterException(string errorResourceId, string message) :
            base("Error Resource ID: " + errorResourceId + ", message: " + message)
        {
            this.resourceId = errorResourceId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AXAdapterException"/> class.
        /// </summary>
        /// <param name="errorResourceId"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public AXAdapterException(string errorResourceId, string message, Exception innerException) :
            base("Error Resource ID: " + errorResourceId + ", message: " + message, innerException)
        {
            this.resourceId = errorResourceId;
        }

    }
}
