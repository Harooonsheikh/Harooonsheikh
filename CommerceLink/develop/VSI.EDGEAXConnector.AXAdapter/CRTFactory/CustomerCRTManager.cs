using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class CustomerCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public CustomerCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ErpCustomer CreateCustomer(ErpCustomer customer, long channelId, string storeKey, string requestId)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.CreateCustomer(customer, channelId, requestId);
        }
        public ErpCustomer UpdateCustomer(ErpCustomer customer, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.UpdateCustomer(customer);
        }
        public ErpUpdateCustomerContactPersonResponse UpdateMergeCustomer(ErpCustomer customer, string erpCustomerAccountNumber, ErpContactPerson erpContactPerson, string storeKey, string requestId)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.UpdateMergeCustomer(customer, erpCustomerAccountNumber, erpContactPerson, requestId);
        }
        public ErpCustomer GetCustomer(string AccountNuber, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.GetCustomer(AccountNuber);
        }

        public ErpCustomer GetCustomerData(string AccountNuber, int searchLocation, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.GetCustomerData(AccountNuber, searchLocation);
        }
        public List<ErpCustomer> GetCustomerByLicence(List<string> licenceNumber, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.GetCustomerByLicence(licenceNumber);
        }

        public ErpCustomerInfoResponse GetCustomerInfoByInvoiceId(CustomerByInvoiceRequest request, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.GetCustomerInfoByInvoiceId(request);
        }

        public ErpCustomerInoviceDetailResponse GetCustomerInvoiceDetails(CustomerInvoiceDetailRequest request, string storeKey, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600000, requestId, "CreateCustomerController", DateTime.UtcNow);
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600001, requestId, "CreateCustomerController", DateTime.UtcNow);

            return customerController.GetCustomerInvoiceDetails(request, requestId);
        }
        public ErpCustomerPaymentInfo GetCustomerPaymentMethods(ErpGetCardRequest erpGetCardRequest, string storeKey, string requestId)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.GetCustomerPaymentMethods(erpGetCardRequest, requestId);
        }
        public ErpCreditCardResponse CreateCustomerPaymentMethod(ErpCreateCardRequest erpCreateCardRequest, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.CreateCustomerPaymentMethod(erpCreateCardRequest);
        }
        public ErpCreditCardResponse UpdateCustomerPaymentMethod(ErpEditCardRequest erpEditCardRequest, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.UpdateCustomerPaymentMethod(erpEditCardRequest);
        }

        public ErpCustomerInoviceResponse GetCustomerInvoices(CustomerInvoiceRequest request, string storeKey, string requestId)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.GetCustomerInvoices(request, requestId);
        }
        public ErpDeleteCustomerPaymentMethodResponse DeleteCustomerPaymentMethod(ErpDeleteCustomerPaymentMethodRequest request, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.DeleteCustomerPaymentMethod(request);
        }

        public ErpCustomerBankAccountResponse CreateCustomerBankAccount(ErpCreateCardRequest erpRequest, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.CreateCustomerBankAccount(erpRequest);
        }

        public ErpTriggerDataSyncResponse TriggerDataSync(string requestXML, string storeKey)
        {
            var customerController = _crtFactory.CreateCustomerController(storeKey);
            return customerController.TriggerDataSync(requestXML);
        }

    }
}
