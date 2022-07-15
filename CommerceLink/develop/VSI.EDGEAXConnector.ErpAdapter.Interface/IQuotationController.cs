using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IQuotationController
    {

        List<ErpCreateCustomerQuotationResponse> CreateCustomerQuotations(List<ErpCustomerOrderInfo> customerQuotations,string storeKey);
        ErpGetCustomerQuotationResponse GetCustomerQuotation(string custAccount, string status, string offerType, string quotationId, string storeKey, string requestId);
        ErpCreateCustomerQuotationResponse CreateCustomerQuotation(ErpCustomerOrderInfo customerQuotation, string storeKey,string requestId);
        ERPQuotationReasonGroupsResponse GetQuotationReasonGroups(string storeKey);
        ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation(string quotationId, string storeKey,string requestId);
        ErpRejectCustomerQuotationResponse RejectCustomerQuotation(string quotationId, string reasonCode, string storeKey);
        ErpConfirmQuotationResponse ConfirmQuotation(ErpConfirmQuotationRequest erpRequest, string requestId);
        ErpQuoteOpportunityUpdateResponse QuoteOpportunityUpdate(ErpQuoteOpportunityUpdateRequest erpRequest);
    }
}
