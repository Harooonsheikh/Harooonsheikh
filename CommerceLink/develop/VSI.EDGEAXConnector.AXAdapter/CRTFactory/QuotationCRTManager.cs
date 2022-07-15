using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class QuotationCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public QuotationCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ErpGetCustomerQuotationResponse GetCustomerQuotation(string custAccount, string status, string offerType, string quotationId, string storeKey, string requestId)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.GetCustomerQuotation(custAccount, status, offerType, quotationId, requestId);
        }

        public ErpCreateCustomerQuotationResponse CreateCustomerQuotation(ErpCustomerOrderInfo customerQuotation, string storeKey, string requestId)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.CreateCustomerQuotation(customerQuotation, requestId);
        }

        public ERPQuotationReasonGroupsResponse GetQuotationReasonGroups( string storeKey)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.GetQuotationReasonGroups();
        }

        public ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation(string quotationId, string storeKey, string requestId)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.ConfirmCustomerQuotation(quotationId,requestId);
        }

        public ErpRejectCustomerQuotationResponse RejectCustomerQuotation(string quotationId, string reasonCode, string storeKey)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.RejectCustomerQuotation(quotationId, reasonCode);
        }

        public List<ErpCreateCustomerQuotationResponse> CreateCustomerQuotations(List<ErpCustomerOrderInfo> customerQuotations, string storeKey)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.CreateCustomerQuotations(customerQuotations);
        }

        public ErpConfirmQuotationResponse ConfirmQuotation(ErpConfirmQuotationRequest request, string storeKey, string requestId)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.ConfirmQuotation(request, requestId);
        }
        public ErpQuoteOpportunityUpdateResponse QuoteOpportunityUpdate(ErpQuoteOpportunityUpdateRequest request, string storeKey)
        {
            var quotationController = _crtFactory.CreateQuotationController(storeKey);
            return quotationController.QuoteOpportunityUpdate(request);
        }

    }
}
