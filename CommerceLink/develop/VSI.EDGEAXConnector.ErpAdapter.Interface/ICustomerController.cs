using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface ICustomerController
    {
        ErpCustomer AssignCustomer(ErpCustomer customer, string requestId, bool isCreateCustomer = true);
        void CreateCustomer(List<ErpCustomer> obj);

        void CreateCustomer(List<ErpCustomer> obj, bool CreateIntegrationKey );

        void GetCustomer(string accountNo);

        ErpCustomer GetCustomerData(string accountNo, int searchLocation);

        void GetUpdatedCustomers();

        List<ErpCustomer> GetUpdatedCustomersAndAddresses(DateTime dateTime, string CustGroup);

        List<ErpAddress> GetUpdatedAddresses(DateTime dateTime, string CustGroup);

        Dictionary<string, long> GetAllCustomersWithIdwithAccountNumber();

        void ProcessDeletedAddresses(List<int> adds);

        ErpCustomer UpdateCustomer(ErpCustomer erpCustomer);
        ErpUpdateCustomerContactPersonResponse MergeUpdateCustomer(ErpCustomer erpCustomer, string ErpCustomerAccountNumber, ErpContactPerson erpContactPerson, string requestId);
        List<ErpCustomer> GetCustomers(List<string> customerAccounts);
        List<ErpCustomer> GetCustomerByLicence(List<string> licenceNumber);
        ErpCustomerInfoResponse GetCustomerInfoByInvoiceId(CustomerByInvoiceRequest request);
        ErpCustomerInoviceDetailResponse GetCustomerInvoiceDetails(CustomerInvoiceDetailRequest request, string requestId);
        ErpCustomerPaymentInfo GetCustomerPaymentMethods(ErpGetCardRequest erpGetCardRequest, string requestId);
        ErpCreditCardResponse CreateCustomerPaymentMethod(ErpCreateCardRequest erpCreateCardRequest, string requestId);
        ErpCreditCardResponse UpdateCustomerPaymentMethod(ErpEditCardRequest erpEditCardRequest, string requestId);
        ErpCustomerInoviceResponse GetCustomerInvoices(CustomerInvoiceRequest request, string requestId);
        ErpDeleteCustomerPaymentMethodResponse DeleteCustomerPaymentMethod(ErpDeleteCustomerPaymentMethodRequest request);
        ErpCustomerBankAccountResponse CreateCustomerBankAccount(ErpCreateCardRequest erpRequest);

        //Custom for TV Third Party Integration
        //string CreateCustomerFromSalesOrder(ErpSalesOrder salesOrderParam);
        ErpCustomer CreateCustomerThridParty(ErpCustomer customer, string salesOrderCurrencyCode);
        ErpTriggerDataSyncResponse TriggerDataSync(string requestXML);
    }
}
