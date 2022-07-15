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
    public interface ICustomerController
    {
        ErpCustomer CreateCustomer(ErpCustomer customer, long channelId, string requestId);
        ErpCustomer UpdateCustomer(ErpCustomer customer);
        ErpUpdateCustomerContactPersonResponse UpdateMergeCustomer(ErpCustomer customer, string erpCustomerAccountNumber, ErpContactPerson erpContactPerson, string requestId);
        ErpCustomer GetCustomer(string AccountNuber);
        ErpCustomer GetCustomerData(string AccountNuber, int searchLocation);
        List<ErpCustomer> GetCustomerByLicence(List<string> licenceNumber);
        ErpCustomerInfoResponse GetCustomerInfoByInvoiceId(CustomerByInvoiceRequest request);
        ErpCustomerInoviceDetailResponse GetCustomerInvoiceDetails(CustomerInvoiceDetailRequest request, string requestId);
        ErpCustomerInoviceResponse GetCustomerInvoices(CustomerInvoiceRequest request, string requestId);
        ErpCustomerPaymentInfo GetCustomerPaymentMethods(ErpGetCardRequest erpGetCardRequest, string requestId);
        ErpCreditCardResponse CreateCustomerPaymentMethod(ErpCreateCardRequest erpCreateCardRequest);
        ErpCreditCardResponse UpdateCustomerPaymentMethod(ErpEditCardRequest erpEditCardRequest);
        ErpDeleteCustomerPaymentMethodResponse DeleteCustomerPaymentMethod(ErpDeleteCustomerPaymentMethodRequest request);
        ErpCustomerBankAccountResponse CreateCustomerBankAccount(ErpCreateCardRequest erpCreateCardRequest);
        ErpTriggerDataSyncResponse TriggerDataSync(string requestXML);
    }
}
