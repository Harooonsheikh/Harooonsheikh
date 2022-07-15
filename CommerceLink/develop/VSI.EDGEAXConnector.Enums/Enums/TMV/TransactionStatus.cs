using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums.Enums.TMV
{
    public enum TransactionStatus
    {
        None = 0,
        Created = 1,
        PreparingToSendToErp = 2,
        SentToERP = 3,
        CreatedInERP = 4,
        CancelledInERP = 5,
        SynchedWithThirdParty = 6,
        SynchedWithThirdPartyFailure = 7,
        SaleOrderAlreadyProcessed = 8,
        CustomerOrResellerCreationFailed = 9,
        SalesOrderCreationFailed = 10,
        ErrorInSalesOrderSycnhing = 11,
        ProductPricesCallFailed = 12,
        DistributerNotFound = 13,
        CanceledIngramStatusUpdation = 14,
        CreatedInERPFailedLimitExceed = 15,
        ErpOrderCreationRetryLimitExceed = 16,

        /// <summary>
        /// Preparing to send Ingram cancel request to D365
        /// </summary>
        IngramCancelRequest_PreparingSyncToErp = 18,

        /// <summary>
        /// Ingram cancel request failed to sync with D365
        /// </summary>
        IngramCancelRequest_SyncToErpFailed = 19,

        /// <summary>
        /// Ingram cancel request sucessfully synced with D365 and order canceled in D365
        /// </summary>
        IngramCancelRequest_OrderCanceledInErp = 20,

        /// <summary>
        /// Status of Ingram cancel request updated in Ingram portal
        /// </summary>
        IngramCancelRequest_StatusUpdatedInThirdParty = 21,

        /// <summary>
        /// Status of Ingram cancel request updated in Ingram portal that order was not canceled or terminated in ERP due to expired possible terminatation date
        /// </summary>
        IngramCancelRequest_StatusUpdatedInThirdPartyOrderTerminationDateExpired = 22,

        /// <summary>
        /// Status of Ingram cancel request failed to update in Ingram portal
        /// </summary>
        IngramCancelRequest_StatusUpdateInThirdPartyFailure = 23,

        /// <summary>
        /// Ingram cancel request failed to sync with D365 due to expired termination date
        /// </summary>
        IngramCancelRequest_SyncToErpFailedDueToExpiredTerminationDate = 24,

        /// <summary>
        /// Ingram cancel request not updated in Ingram portal
        /// </summary>
        IngramCancelRequest_StatusForThirdPartyNotFound = 25,

        /// <summary>
        /// Transfer Ingram order successfully synch with Third party.
        /// </summary>
        TransferIngramRequest_OrderTransferedSynchInThirdParty = 50,

        /// <summary>
        /// Transfer Ingram order fail and retry with third party for next attempt.
        /// </summary>
        TransferIngramRequest_OrderTransferedSynchInThirdPartyFailed = 51,

        /// <summary>
        /// Transfer Ingram order successfully transfered in D365.
        /// </summary>
        TransferIngramRequest_OrderTransfered = 52,

        /// <summary>
        /// Transfer Ingram order validation fail in D365 and will Synch back to ingram.
        /// </summary>
        TransferIngramRequest_ValidationFailed = 53,

        /// <summary>
        /// Transfer Ingram order fail and retry in D365 for next attempt.
        /// </summary>
        TransferIngramRequest_Other = 54,
        
        /// <summary>
        /// Transfer Ingram order code not found.
        /// </summary>
        TransferIngramRequest_None = 55,

        /// <summary>
        /// Transfer Ingram order code not found.
        /// </summary>
        MissingParameter_EndCustomerAdminEmail = 70,
        
        /// <summary>
        /// Transfer Ingram order code not found.
        /// </summary>
        MissingParameter_IngramUpdateParameterFailed = 71,

        /// <summary>
        /// Transfer Ingram order code not found.
        /// </summary>
        MissingParameter_IngramUpdateOrderStatusInquireFailed = 72,

        MissingParameter_IngramStatusUpdateLimitExceed = 75,

        MissingParameter_IngramOrderMarkedDeleted = 76


    }
}
