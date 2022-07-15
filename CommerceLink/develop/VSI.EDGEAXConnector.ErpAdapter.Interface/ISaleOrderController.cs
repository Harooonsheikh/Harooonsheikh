using System;
using System.Collections.Generic;
using VSI.CommerceLink.EcomDataModel;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels.Enums;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface ISaleOrderController
    {
        object SearchSalesOrder(string orderNumber);

        bool CreateSaleOrders(ErpSalesOrder salesOrder, string requestId);

        bool CreateRealtimeSaleOrderTransaction(ErpSalesOrder salesOrder, string requestId);

        List<ErpSalesOrderStatus> GetSalesOrderStatusUpdate();

        /// <summary>
        /// This method is the overload of legacy GetSalesOrderStatusUpdate. When usingCRT is false, it get results from the legacy GetSalesOrderStatusUpdate method otherwise would use Commerce Runtime OrderManager API to retrieve order statuses.
        /// </summary>
        /// <param name="usingCRT">
        /// /// false to use Dynamics Realtime service extension. 
        /// true to use OrderManager Commerce Runtime class SearchOrders method. </param>
        /// <returns> List of ErpSalesOrderStatus </returns>
        List<ErpSalesOrderStatus> GetSalesOrderStatusUpdate(bool usingCRT);

        void UpdateStatusIntegration(Dictionary<string, string> updatedOrder);
        
        /// <summary>
        /// Get Retail Affiliation
        /// </summary>
        /// <returns></returns>
        List<ErpAffiliation> GetRetailAffiliations();

        ERPContractSalesorderResponse GetContractSalesOrder(ContractSalesorderRequest request,string requestId);
        double CalculateLineAmount(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, int _salesQty,
            double _salesPrice, double _discAmount, double _discPct, bool _isSubscription = false, bool _tmvAutoProlongation = false);

        double CalculateTimeQty(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, bool _isSubscription = false, 
            bool _tmvAutoProlongation = false);

        ErpContractInvoicesResponse GetContractInvoices(ContractInvoicesRequest request);

        ErpValidateVATNumberResponse ValidateVATNumber(ErpValidateVATNumberRequest request, string requestId);

        ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoice(ErpAddPaymentLinkForInvoiceRequest request, string requestId);

        ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoiceBoleto(ErpAddPaymentLinkForInvoiceBoletoRequest request, string requestId);

        ErpCreateLicenseResponse CreateProductLicense(List<ErpCreateActionLinkRequest> request, string requestId);

        ErpCloseExistingOrderResponse CloseExistingOrder(string salesId, string pacLicense, string disablePacLicenseOfSalesLines = "");

        ErpChangeContractPaymentMethodResponse ChangeContractPaymentMethod(string salesId, long newPaymentMethodRecId, string tenderTypeId, long bankAccountRecId);

        ProcessContractOperationResponse ProcessContractOperation(ProcessContractOperationRequest request, List<ErpTenderLine> tenderLines, bool isCheckoutProcessContractOperation, string requestId);

        PriceResponse GetOrValidatePriceInformation(PriceRequest request, bool IsErpCustomer = false);

        ErpReactivateContract ReactivateContract(string pacLicenseList, string subscriptionStartDate);

        ErpCancelIngramOrderResponse CancelIngramOrder(string prNumber, string salesId, DateTimeOffset orderDate);

        ErpChangeIngramOrderResponse ChangeIngramOrder(string salesOrderXML);

        ErpCreatePaymentJournalResponse CreatePaymentJournal(ErpCreatePaymentJournalRequest request, string requestId);

        ErpTransferPartnerContractResponse TransferPartnerContract(TransferPartnerContractRequest request, string requestId);
        ErpTransferIngramOrderResponse TransferIngramOrder(string salesOrderXML);

        ErpContractRenewalResponse ContractRenewal(ContractRenewalRequest request, string requestId);

        ErpGetBoletoUrlResponse GetBoletoUrl(ErpGetBoletoUrlRequest request, string requestId);
        ErpUpdateCustomerPortalLinkResponse UpdateCustomerPortalLink(UpdateCustomerPortalLinkRequest updateCustomerPortalLinkRequest, string storeKey);
    }
}
