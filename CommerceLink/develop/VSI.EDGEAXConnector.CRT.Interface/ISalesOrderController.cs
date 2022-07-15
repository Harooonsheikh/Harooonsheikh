using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels.Enums;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface ISalesOrderController
    {
        object SearchOrders(string orderNumber);

        List<ErpSalesOrderStatus> GetSalesOrderStatus(string channelReferenceIds, string salesIds, DateTimeOffset fromDateOff, DateTimeOffset toDateOff);

        List<ErpSalesOrderStatus> GetSalesOrderRenewalStatus(string salesIds);

        ErpSalesOrder UploadOrder(ErpSalesOrder salesOrder, string requestId);

        ERPContractSalesorderResponse GetContractSalesOrder(ContractSalesorderRequest request, string requestId);

        double CalculateLineAmount(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, int _salesQty, double _salesPrice,
            double _discAmount, double _discPct, bool _isSubscription = false, bool _tmvAutoProlongation = false);

        double CalculateTimeQty(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, bool _isSubscription = false,
            bool _tmvAutoProlongation = false);

        ErpContractInvoicesResponse GetContractInvoices(ContractInvoicesRequest request);

        ErpValidateVATNumberResponse ValidateVATNumber(ErpValidateVATNumberRequest request, string requestId);

        List<ErpAffiliation> GetRetailAffiliations(string storeKey);

        ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoice(ErpAddPaymentLinkForInvoiceRequest request, string requestId);
        ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoiceBoleto(ErpAddPaymentLinkForInvoiceBoletoRequest request, string requestId);

        ErpCreateLicenseResponse CreateProductLicense(List<ErpCreateActionLinkRequest> request, string requestId);

        ErpCloseExistingOrderResponse CloseExistingOrder(string salesId, string pacLicense, string disablePacLicenseOfSalesLines = "");

        ErpChangeContractPaymentMethodResponse ChangeContractPaymentMethod(string salesId, long newPaymentMethodRecId, string tenderTypeId, long bankAccountRecId);
        ProcessContractOperationResponse ProcessContractOperation(string contractSwitchMigrateRequest);
        ProcessContractOperationResponse CheckoutProcessContractOperation(string contractSwitchMigrateRequest);
        PriceResponse GetOrValidatePriceInformation(PriceRequest priceValidateRequest);
        ProcessContractOperationResponse CreateNewContractLines(string createNewContractLinesRequest);
        ErpReactivateContract ReactivateContract(string pacLicenseList, string subscriptionStartDate);

        ErpCancelIngramOrderResponse CancelIngramOrder(string prNumber, string salesId, DateTimeOffset orderDate);

        ErpChangeIngramOrderResponse ChangeIngramOrder(string salesOrderXML);
        ErpCreatePaymentJournalResponse CreatePaymentJournal(ErpCreatePaymentJournalRequest request, string requestId);

        ErpTransferPartnerContractResponse TransferPartnerContract(TransferPartnerContractRequest request, string requestId);

        ErpTransferIngramOrderResponse TransferIngramOrder(string salesOrderXML);

        ErpContractRenewalResponse ContractRenewal(ContractRenewalRequest request, string requestId);

        ErpGetBoletoUrlResponse GetBoletoUrl(ErpGetBoletoUrlRequest request, string requestId);
        ErpUpdateCustomerPortalLinkResponse UpdateCustomerPortalLink(UpdateCustomerPortalLinkRequest updateCustomerPortalLinkRequest);
    }
}
