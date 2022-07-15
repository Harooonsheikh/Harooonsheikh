using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class SalesOrderCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public SalesOrderCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public object SearchOrders(string orderNumber, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.SearchOrders(orderNumber);
        }

        public ErpSalesOrder UploadOrder(ErpSalesOrder salesOrder, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.UploadOrder(salesOrder, requestId);
        }

        public List<ErpSalesOrderStatus> GetSalesOrderStatus(string channelReferenceIds, string salesIds, DateTimeOffset fromDateOff, DateTimeOffset toDateOff, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.GetSalesOrderStatus(channelReferenceIds, salesIds, fromDateOff, toDateOff);
        }

        public List<ErpSalesOrderStatus> GetSalesOrderRenewalStatus(string salesIds, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.GetSalesOrderRenewalStatus(salesIds);
        }

        public ERPContractSalesorderResponse GetContractSalesOrder(ContractSalesorderRequest request, string storeKey, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600000, requestId, "CreateSalesOrderController", DateTime.UtcNow);
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600001, requestId, "CreateSalesOrderController", DateTime.UtcNow);

            return salesOrderController.GetContractSalesOrder(request, requestId);
        }

        public double CalculateLineAmount(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, int _salesQty, double _salesPrice, double _discAmount, double _discPct, string storeKey, bool _isSubscription = false, bool _tmvAutoProlongation = false)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CalculateLineAmount(_calculateDateFrom, _validTo, _billingPeriod, _salesQty, _salesPrice, _discAmount, _discPct, _isSubscription, _tmvAutoProlongation);
        }

        public double CalculateTimeQty(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, string storeKey,bool _isSubscription = false, bool _tmvAutoProlongation = false)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CalculateTimeQty(_calculateDateFrom, _validTo, _billingPeriod, _isSubscription, _tmvAutoProlongation);
        }

        public ErpContractInvoicesResponse GetContractInvoices(ContractInvoicesRequest request, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.GetContractInvoices(request);
        }

        public ErpValidateVATNumberResponse ValidateVATNumber(ErpValidateVATNumberRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.ValidateVATNumber(request, requestId);
        }

        public List<ErpAffiliation> GetRetailAffiliations(string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.GetRetailAffiliations(storeKey);
        }

        public ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoice(ErpAddPaymentLinkForInvoiceRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.AddPaymentLinkForInvoice(request, requestId);
        }

        public ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoiceBoleto(ErpAddPaymentLinkForInvoiceBoletoRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.AddPaymentLinkForInvoiceBoleto(request, requestId);
        }

        public ErpCreateLicenseResponse CreateProductLicense(List<ErpCreateActionLinkRequest> request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CreateProductLicense(request, requestId);
        }

        public ErpCloseExistingOrderResponse CloseExistingOrder(string salesId, string pacLicense, string storeKey, string disablePacLicenseOfSalesLines = "")
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CloseExistingOrder(salesId, pacLicense, disablePacLicenseOfSalesLines);
        }

        public ErpChangeContractPaymentMethodResponse ChangeContractPaymentMethod(string salesId, long newPaymentMethodRecId, string tenderTypeId, long bankAccountRecId, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.ChangeContractPaymentMethod(salesId, newPaymentMethodRecId, tenderTypeId, bankAccountRecId);
        }

        public ProcessContractOperationResponse ProcessContractOperation(string processContractOperationRequest, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.ProcessContractOperation(processContractOperationRequest);
        }

        public ProcessContractOperationResponse CheckoutProcessContractOperation(string request, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CheckoutProcessContractOperation(request);
        }

        public PriceResponse GetOrValidatePriceInformation(PriceRequest priceValidateRequest, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.GetOrValidatePriceInformation(priceValidateRequest);
        }
        public ProcessContractOperationResponse CreateNewContractLines(string createNewContractLinesRequest, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CreateNewContractLines(createNewContractLinesRequest);
        }

        public ErpReactivateContract ReactivateContract(string pacLicenseList, string subscriptionStartDate, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.ReactivateContract(pacLicenseList, subscriptionStartDate);
        }

        public ErpCancelIngramOrderResponse CancelIngramOrder(string prNumber, string salesId, DateTimeOffset orderDate, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CancelIngramOrder(prNumber, salesId, orderDate);
        }

        public ErpChangeIngramOrderResponse ChangeIngramOrder(string salesOrderXML, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.ChangeIngramOrder(salesOrderXML);
        }

        public ErpCreatePaymentJournalResponse CreatePaymentJournal(ErpCreatePaymentJournalRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.CreatePaymentJournal(request, requestId);
        }


        public ErpTransferPartnerContractResponse TransferPartnerContract(TransferPartnerContractRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.TransferPartnerContract(request, requestId);
        }

        public ErpTransferIngramOrderResponse TransferIngramOrder(string salesOrderXML, string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.TransferIngramOrder(salesOrderXML);
        }
        public ErpContractRenewalResponse ContractRenewal(ContractRenewalRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.ContractRenewal(request, requestId);
        }

        public ErpGetBoletoUrlResponse GetBoletoUrl(ErpGetBoletoUrlRequest request, string storeKey, string requestId)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.GetBoletoUrl(request, requestId);
        }
        public ErpUpdateCustomerPortalLinkResponse UpdateCustomerPortalLink(UpdateCustomerPortalLinkRequest updateCustomerPortalLinkRequest,string storeKey)
        {
            var salesOrderController = _crtFactory.CreateSalesOrderController(storeKey);
            return salesOrderController.UpdateCustomerPortalLink(updateCustomerPortalLinkRequest);
        }
    }
}
