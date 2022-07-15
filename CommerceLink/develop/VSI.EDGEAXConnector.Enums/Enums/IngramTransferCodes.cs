namespace VSI.EDGEAXConnector.Enums.Enums
{
    /// <summary>
    /// Ingram transfer order codes
    /// </summary>
    public enum IngramTransferCodes
    {
        /// <summary>
        /// No code provided
        /// </summary>
        None,

        /// <summary>
        /// Successfully transfered order
        /// </summary>
        OrderTransfered,

        /// <summary>
        /// Any validation failed, that need to synch back to Ingram as order failed.
        /// </summary>
        ValidationFailed,

        /// <summary>
        /// Transfer failed due to Exception or other reasons and order need to retry for transfer.
        /// </summary>
        Other
    }
}
