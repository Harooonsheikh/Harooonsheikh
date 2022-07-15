namespace VSI.EDGEAXConnector.Enums.Enums
{
    /// <summary>
    /// Ingram order cancellation termination codes
    /// </summary>
    public enum IngramCancelTerminationCodes
    {
        /// <summary>
        /// Cancellation successfull on ingram order
        /// </summary>
        Cancelled,

        /// <summary>
        /// Termination successfull on ingram order
        /// </summary>
        Terminated,

        /// <summary>
        /// Termination operation on ingram order failed due to expiration of possible termination date
        /// </summary>
        TerminationDateExpired,

        /// <summary>
        /// Cancellation or Termination failed due to other reasons
        /// </summary>
        Other
    }
}
