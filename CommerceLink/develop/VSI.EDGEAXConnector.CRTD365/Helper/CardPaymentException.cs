using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using System;
using System.Collections.Generic;

namespace VSI.EDGEAXConnector.CRTD365.Helper
{
    public class CardPaymentException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPaymentException" /> class.
        /// </summary>
        public CardPaymentException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPaymentException" /> class.
        /// </summary>
        /// <param name="message">The message describing the error that occurred.</param>
        public CardPaymentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CardPaymentException" /> class.
        /// </summary>
        /// <param name="message">The message describing the error that occurred.</param>
        /// <param name="paymentErrors">The payment errors causing the error.</param>
        public CardPaymentException(string message, IEnumerable<PaymentError> paymentErrors)
            : base(message)
        {
            this.PaymentErrors = paymentErrors;
        }

        /// <summary>
        /// Gets or sets payment errors.
        /// </summary>
        public IEnumerable<PaymentError> PaymentErrors { get; set; }
    }
}
