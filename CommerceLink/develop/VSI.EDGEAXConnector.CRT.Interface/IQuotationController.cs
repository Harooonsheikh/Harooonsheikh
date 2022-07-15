using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IQuotationController
    {
        ErpGetCustomerQuotationResponse GetCustomerQuotation(string custAccount, string status, string offerType, string quotationId, string requestId);
        ErpCreateCustomerQuotationResponse CreateCustomerQuotation(ErpCustomerOrderInfo customerQuotation,string requestId);
        ERPQuotationReasonGroupsResponse GetQuotationReasonGroups();
        ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation(string quotationId,string requestId);
        ErpRejectCustomerQuotationResponse RejectCustomerQuotation(string quotationId, string reasonCode);
        List<ErpCreateCustomerQuotationResponse> CreateCustomerQuotations(List<ErpCustomerOrderInfo> customerQuotations);
        ErpConfirmQuotationResponse ConfirmQuotation(ErpConfirmQuotationRequest request, string requestId);
        ErpQuoteOpportunityUpdateResponse QuoteOpportunityUpdate(ErpQuoteOpportunityUpdateRequest erpRequest);
    }
}
