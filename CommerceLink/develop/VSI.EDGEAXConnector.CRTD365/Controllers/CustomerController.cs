using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Collections.Generic;
using Microsoft.Dynamics.Commerce.RetailProxy;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using System;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using System.Linq;
using System.Collections.ObjectModel;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using System.Diagnostics;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class CustomerController : BaseController, ICustomerController
    {
        public CustomerController(string storeKey) : base(storeKey)
        {

        }

        /// [MB] - TV - BR 3.0 - 15007 - Start
        /// <summary>
        /// Method used to add extension properties in RS customer object from ERP customer object
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="objCustomer"></param>
        /// <returns></returns>
        private Customer addExtensionProperties(Customer customer, ErpCustomer objCustomer)
        {
            CommerceProperty commerceProperty;
            CommercePropertyValue commercePropertyValue;

            foreach (ErpCommerceProperty extProp in objCustomer.ExtensionProperties)
            {
                //customer.ExtensionProperties = new System.Collections.ObjectModel.ObservableCollection<CommerceProperty>();

                commerceProperty = new CommerceProperty();
                commerceProperty.Key = extProp.Key;

                commercePropertyValue = new CommercePropertyValue();
                commercePropertyValue.BooleanValue = extProp.Value.BooleanValue;
                commercePropertyValue.ByteValue = extProp.Value.ByteValue;
                commercePropertyValue.DateTimeOffsetValue = extProp.Value.DateTimeOffsetValue;
                commercePropertyValue.DecimalValue = extProp.Value.DecimalValue;
                commercePropertyValue.IntegerValue = extProp.Value.IntegerValue;
                commercePropertyValue.LongValue = extProp.Value.LongValue;
                commercePropertyValue.StringValue = extProp.Value.StringValue;

                commerceProperty.Value = commercePropertyValue;

                customer.ExtensionProperties.Add(commerceProperty);
            }
            return customer;
        }
        /// [MB] - TV - BR 3.0 - 15007 - End

        public ErpCustomer CreateCustomer(ErpCustomer objCustomer, long channelId, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            bool isExternalSystemTimeLogged = false;

            Customer customer = new Customer();
            Customer objCustomerReturn;
            ErpCustomer objErpCustomerReturn;
            try
            {
                objCustomer.Addresses = objCustomer.Addresses.OrderByDescending(a => a.IsPrimary).ToList();

                // Map the ErpCustomer object (passed in parmeter) to Customer object
                customer = _mapper.Map<ErpCustomer, Customer>(objCustomer);

                // [MB] - TV - BR 3.0 - 15007 - CRM customer changes in createCustomerAPI - Start
                customer = this.addExtensionProperties(customer, objCustomer);
                // [MB] - TV - BR 3.0 - 15007 - CRM customer changes in createCustomerAPI - End

                AddMandatoryStateExtensionProperty(customer);

                string externalIdentityId = objCustomer.EcomCustomerId;
                //string externalIdentityIssuer = "VW";
                string externalIdentityIssuer = string.Empty;
                //++RSCall -  Create new customer
                var extensionProperties = customer.ExtensionProperties;
                customer.ExtensionProperties = new ObservableCollection<CommerceProperty>();
                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_CreateNewCustomer", DateTime.UtcNow);
                objCustomerReturn = ECL_CreateNewCustomer(customer, externalIdentityId, externalIdentityIssuer, extensionProperties);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateNewCustomer", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_CreateNewCustomer", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                // Map returned Customer object to ErpCustomer object
                objErpCustomerReturn = _mapper.Map<Customer, ErpCustomer>(objCustomerReturn);

                //[MB] - Custom for TV - 14719 - Customer address type string value - Start
                for (int i = 0; i < objErpCustomerReturn.Addresses.Count(); i++)
                {
                    objErpCustomerReturn.Addresses[i].AddressType = (ErpAddressType)objErpCustomerReturn.Addresses[i].AddressTypeValue;
                    objErpCustomerReturn.Addresses[i].AddressTypeStrValue = Enum.GetName(typeof(ErpAddressType), objErpCustomerReturn.Addresses[i].AddressTypeValue);
                }
                //[MB] - Custom for TV - 14719 - Customer address type string value - End
            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "CreateNewCustomer", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_CreateNewCustomer", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception ex)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "CreateNewCustomer", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_CreateNewCustomer", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            // return the returned ErpCustomer object
            return objErpCustomerReturn;
        }  
        public ErpUpdateCustomerContactPersonResponse UpdateMergeCustomer(ErpCustomer erpCustomer, string ErpCustomerAccountNumber, ErpContactPerson erpContactPerson, string requestId)
        {
            erpCustomer.Addresses = erpCustomer.Addresses.OrderByDescending(a => a.IsPrimary).ToList();

            bool isExternalSystemTimeLogged = false;
            Customer customer = new Customer();
            customer = _mapper.Map<ErpCustomer, Customer>(erpCustomer);

            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - Start
            customer = this.addExtensionProperties(customer, erpCustomer);
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in updateCustomerAPI - End

            ContactPerson conPerson = new ContactPerson();
            conPerson = _mapper.Map<ErpContactPerson, ContactPerson>(erpContactPerson);
            ErpUpdateCustomerContactPersonResponse erpUpdateCustomerContactPersonResponse = new ErpUpdateCustomerContactPersonResponse(false, "", null, null);
            try
            {
                AddMandatoryStateExtensionProperty(customer);
                var extensionProperties = customer.ExtensionProperties;
                customer.ExtensionProperties = new ObservableCollection<CommerceProperty>();
                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_UpdateCustomerContactPerson", DateTime.UtcNow);
                var result = ECL_UpdateCustomerContactPerson(ErpCustomerAccountNumber, customer, conPerson, extensionProperties);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "UpdateCustomerContactPerson", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_UpdateCustomerContactPerson", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)result.Status)
                {
                    ErpCustomer deserializeCustomer = JsonConvert.DeserializeObject<ErpCustomer>(result.Customer);
                    ErpContactPerson deserializeContactPerson = JsonConvert.DeserializeObject<ErpContactPerson>(result.ContactPerson);
                    erpUpdateCustomerContactPersonResponse = new ErpUpdateCustomerContactPersonResponse((bool)result.Status, result.Message, deserializeCustomer, deserializeContactPerson);
                }
                else if (!(bool)result.Status)
                {
                    erpUpdateCustomerContactPersonResponse = new ErpUpdateCustomerContactPersonResponse(false, result.Message, null, null);
                }

            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "UpdateCustomerContactPerson", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_UpdateCustomerContactPerson", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception ex)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "UpdateCustomerContactPerson", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_UpdateCustomerContactPerson", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            return erpUpdateCustomerContactPersonResponse;
        }
        public ErpCustomer UpdateCustomer(ErpCustomer erpCustomer)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            erpCustomer.Addresses = erpCustomer.Addresses.OrderByDescending(a => a.IsPrimary).ToList();

            Customer customer = new Customer();
            customer = _mapper.Map<ErpCustomer, Customer>(erpCustomer);
            
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in createCustomerAPI - Start
            customer = this.addExtensionProperties(customer, erpCustomer);
            // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in createCustomerAPI - End
            try
            {
                AddMandatoryStateExtensionProperty(customer);

                var extensionProperties = customer.ExtensionProperties;
                customer.ExtensionProperties = new ObservableCollection<CommerceProperty>();
                // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in createCustomerAPI - Start
                customer = ECL_UpdateCustomer(customer, extensionProperties);
                // [MB] - TV - BR 3.0 - 15878 - CRM customer changes in createCustomerAPI - End
                erpCustomer = _mapper.Map<Customer, ErpCustomer>(customer);
            }
            catch (Exception ex)
            {
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCustomer;
        }
        public ErpCustomer GetCustomer(string accountNuber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            bool isExternalSystemTimeLogged = false;
            ErpCustomer erpCustomer = new ErpCustomer();
            var objCustomerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            try
            {
                Customer customer = new Customer();
                timer = Stopwatch.StartNew();
                customer = ECL_GetCustomer(accountNuber);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, Guid.NewGuid().ToString(), "UpdateCustomerContactPerson", GetElapsedTime());
                isExternalSystemTimeLogged = true;
                erpCustomer = _mapper.Map<Customer, ErpCustomer>(customer);
            }
            catch (Exception ex)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, Guid.NewGuid().ToString(), "UpdateCustomerContactPerson", GetElapsedTime());
                }
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpCustomer;
        }
        public List<ErpCustomer> GetCustomerByLicence(List<string> licenceNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpCustomer> erpCustomers = new List<ErpCustomer>();
            try
            {
                Customer customer = new Customer();

                QueryResultSettings customerQuerySetting = new QueryResultSettings();
                customerQuerySetting.Paging = new PagingInfo();
                customerQuerySetting.Paging.Skip = 0;
                customerQuerySetting.Paging.Top = 100;

                var liceceNumberList = string.Join<string>(",", licenceNumber);

                var customers =  ECL_GetCustomersByLicenses(customerQuerySetting, liceceNumberList);

                //++TODO add customer by looping on them. 
                foreach (var item in customers)
                {
                    erpCustomers.Add(_mapper.Map<Customer, ErpCustomer>(item));
                }


            }
            catch (Exception ex)
            {
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpCustomers;
        }
        public ErpCustomerInfoResponse GetCustomerInfoByInvoiceId(CustomerByInvoiceRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCustomerInfoResponse customerInfoResponse = new ErpCustomerInfoResponse(false, "", null);
            try
            {
                var rsResponse = ECL_TV_GetCustomerInfoByInvoiceId(request);

                if ((bool)rsResponse.Status)
                {
                    CustomerInfo customerInfo = JsonConvert.DeserializeObject<CustomerInfo>(rsResponse.CustomerInfo);
                    customerInfoResponse = new ErpCustomerInfoResponse(true, rsResponse.Message, customerInfo);
                }
                else if (!(bool)rsResponse.Status)
                {
                    customerInfoResponse = new ErpCustomerInfoResponse(false, rsResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            return customerInfoResponse;
        }

        /// <summary>
        /// Method to get the customer invoice by invoiceid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ErpCustomerInoviceDetailResponse GetCustomerInvoiceDetails(CustomerInvoiceDetailRequest request, string requestId)
        {
            ErpCustomerInoviceDetailResponse erpResponse = new ErpCustomerInoviceDetailResponse(false, null, null, "");
            try
            {               
                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_GetCustomerInvoiceByInvoiceId", DateTime.UtcNow);
                var rsResponse = ECL_TV_GetCustomerInvoiceByInvoiceId(request);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetCustomerInvoiceByInvoiceId", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_GetCustomerInvoiceByInvoiceId", DateTime.UtcNow);

                if ((bool)rsResponse.Status)
                {
                    CustomerInvoiceDetail customerInvoice = JsonConvert.DeserializeObject<CustomerInvoiceDetail>(rsResponse.CustomerInvoiceDetails);

                    ErpCustomer customer = JsonConvert.DeserializeObject<ErpCustomer>(rsResponse.CustomerInfo);

                    if (customerInvoice?.CustInvoiceJour != null && !string.IsNullOrEmpty(customerInvoice.CustInvoiceJour.LocalTaxId))
                    {
                        var localTaxIdProperty =
                            customer.ExtensionProperties.FirstOrDefault(a => a.Key == "LocalTaxId");

                        if (localTaxIdProperty == null)
                        {
                            localTaxIdProperty = new ErpCommerceProperty("LocalTaxId",
                                customerInvoice.CustInvoiceJour.LocalTaxId);
                            customer.ExtensionProperties.Add(localTaxIdProperty);
                        }
                        else
                        {
                            localTaxIdProperty.Value = new ErpCommercePropertyValue
                            {
                                StringValue = customerInvoice.CustInvoiceJour.LocalTaxId
                            };
                        }

                    }

                    if (customerInvoice != null & customerInvoice.CustInvoiceJour != null)
                    {

                        ErpCustomerInvoiceDetail erpCustomerInvoice = GetErpCustomerInvoice(customerInvoice);
                        erpResponse = new ErpCustomerInoviceDetailResponse(true, customer, erpCustomerInvoice, "");
                    }
                    else
                    {
                        erpResponse = new ErpCustomerInoviceDetailResponse(true, null, null, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40004));
                    }
                }
                else if (!(bool)rsResponse.Status)
                {
                    erpResponse = new ErpCustomerInoviceDetailResponse(false, null, null, rsResponse.Message, rsResponse.ErrorCode);
                }

            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            return erpResponse;
        }
        /// <summary>
        /// Method to get the customer invoice by invoiceid
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ErpCustomerInoviceResponse GetCustomerInvoices(CustomerInvoiceRequest request, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            bool isExternalSystemTimeLogged = false;
            ErpCustomerInoviceResponse erpResponse = new ErpCustomerInoviceResponse(false, null, "");
            try
            {
                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_GetCustomerInvoices", DateTime.UtcNow);
                var rsResponse = ECL_TV_GetCustomerInvoices(request);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetCustomerInvoices", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_GetCustomerInvoices", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)rsResponse.Status)
                {
                    if (rsResponse.CustomerInvoices != null)
                    {

                        List<ErpCustomerInvoice> erpCustomerInvoices = MapErpCustomerInvoice(rsResponse.CustomerInvoices);
                        erpResponse = new ErpCustomerInoviceResponse(true, erpCustomerInvoices, "");
                    }
                    else
                    {
                        erpResponse = new ErpCustomerInoviceResponse(true, null, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40004));
                    }
                }
                else if (!(bool)rsResponse.Status)
                {
                    erpResponse = new ErpCustomerInoviceResponse(false, null, rsResponse.Message);
                }

            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "GetCustomerInvoices", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_GetCustomerInvoices", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch(Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "GetCustomerInvoices", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_GetCustomerInvoices", DateTime.UtcNow);
                }
                throw exp;
            }
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        /// <summary>
        /// Get Customer Data
        /// </summary>
        /// <param name="AccountNuber"></param>
        /// <returns></returns>
        public ErpCustomer GetCustomerData(string AccountNuber, int searchLocation)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCustomer erpCustomer = new ErpCustomer();
            try
            {
                Customer customer = new Customer();
                if (searchLocation == 1)
                {
                    customer = ECL_GetCustomer(AccountNuber);
                }
                else
                {
                    customer = ECL_RTS_GetCustomer(AccountNuber);
                }

                if (customer == null)
                {
                    throw new Logging.CommerceLinkExceptions.CommerceLinkError(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10302));
                }

                erpCustomer = _mapper.Map<Customer, ErpCustomer>(customer);

                //[MB] - Custom for TV - 14719 - Customer address type string value - Start
                for (int i = 0; i < erpCustomer.Addresses.Count(); i++)
                {
                    erpCustomer.Addresses[i].AddressType = (ErpAddressType)erpCustomer.Addresses[i].AddressTypeValue;
                    erpCustomer.Addresses[i].AddressTypeStrValue = Enum.GetName(typeof(ErpAddressType), erpCustomer.Addresses[i].AddressTypeValue);
                }
                //[MB] - Custom for TV - 14719 - Customer address type string value - End
            }
            catch (Exception ex)
            {
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCustomer;
        }
        public ErpCustomerPaymentInfo GetCustomerPaymentMethods(ErpGetCardRequest erpGetCardRequest, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpCustomerPaymentInfo erpCustomerPaymentInfoResponse = new ErpCustomerPaymentInfo(false, "", null,null);

            try
            {
                string cardProcessors = erpGetCardRequest.cardProcessors.FirstOrDefault();
                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_GetCustomerPaymentMethods", DateTime.UtcNow);
                var customerPaymentInfo = ECL_GetCustomerPaymentMethods(erpGetCardRequest, cardProcessors);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetCustomerPaymentMethods", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_GetCustomerPaymentMethods", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)customerPaymentInfo.Status)
                {
                    List<ErpCreditCardCust> erpCreditCards = new List<ErpCreditCardCust>();
                    List<ErpBankAccountCust> erpBankAccounts = new List<ErpBankAccountCust>();

                    foreach (CreditCardCust creditCard in customerPaymentInfo.CreditCards)
                    {
                        erpCreditCards.Add(_mapper.Map<CreditCardCust, ErpCreditCardCust>(creditCard));
                    }
                    foreach (CustBankAccount bankAccount in customerPaymentInfo.BankAccounts)
                    {
                        erpBankAccounts.Add(_mapper.Map<CustBankAccount, ErpBankAccountCust>(bankAccount));
                    }

                    erpCustomerPaymentInfoResponse = new ErpCustomerPaymentInfo((bool)customerPaymentInfo.Status, customerPaymentInfo.Message, erpCreditCards, erpBankAccounts);
                }
                else
                {
                    erpCustomerPaymentInfoResponse = new ErpCustomerPaymentInfo((bool)customerPaymentInfo.Status, customerPaymentInfo.Message, null, null);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002,currentStore, requestId,"ECL_TV_GetCustomerInvoices", DateTime.UtcNow);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "GetCustomerPaymentMethods", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_GetCustomerPaymentMethods", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpCustomerPaymentInfoResponse = new ErpCustomerPaymentInfo(false, exp.Message, null, null);
            }
            catch(Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "GetCustomerPaymentMethods", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_GetCustomerPaymentMethods", DateTime.UtcNow);
                }
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpCustomerPaymentInfoResponse = new ErpCustomerPaymentInfo(false, exp.Message, null, null);
            }

            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCustomerPaymentInfoResponse;
        }
        public ErpCreditCardResponse CreateCustomerPaymentMethod(ErpCreateCardRequest erpCreateCardRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCreditCardResponse erpResponse = new ErpCreditCardResponse(false, "", null);
            try
            {
                var rsResponse = ECL_CreateCustomerPaymentMethod(erpCreateCardRequest);

                if ((bool)rsResponse.Status)
                {
                    List<ErpCreditCardCust> erpCreditCards = new List<ErpCreditCardCust>();

                    foreach (CreditCardCust creditCard in rsResponse.CreditCards)
                    {
                        erpCreditCards.Add(_mapper.Map<CreditCardCust, ErpCreditCardCust>(creditCard));
                    }

                    erpResponse = new ErpCreditCardResponse(true, rsResponse.Message, erpCreditCards);
                }
                else
                {
                    erpResponse = new ErpCreditCardResponse(false, rsResponse.Message, null);
                }
            }
            catch(Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpResponse = new ErpCreditCardResponse(false, exp.Message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpCreditCardResponse UpdateCustomerPaymentMethod(ErpEditCardRequest erpEditCardRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCreditCardResponse erpResponse = new ErpCreditCardResponse(false, "", null);
            try
            {
                //var rsResponse = ECL_UpadteCustomerPaymentMethod(erpEditCardRequest);

                //if ((bool)rsResponse.Status)
                //{
                //    List<ErpCreditCardCust> erpCreditCards = new List<ErpCreditCardCust>();

                //    foreach (CreditCardCust creditCard in rsResponse.CreditCards)
                //    {
                //        erpCreditCards.Add(_mapper.Map<CreditCardCust, ErpCreditCardCust>(creditCard));
                //    }

                //    erpResponse = new ErpCreditCardResponse(true, rsResponse.Message, erpCreditCards);
                //}
                //else
                //{
                //    erpResponse = new ErpCreditCardResponse(false, rsResponse.Message, null);
                //}
            }
            catch(Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpResponse = new ErpCreditCardResponse(false, exp.Message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpDeleteCustomerPaymentMethodResponse DeleteCustomerPaymentMethod(ErpDeleteCustomerPaymentMethodRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpDeleteCustomerPaymentMethodResponse erpResponse = new ErpDeleteCustomerPaymentMethodResponse(false, "");
            try
            {
                var rsResponse = ECL_TV_DeleteCustomerPaymentMethod(request);

                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ErpDeleteCustomerPaymentMethodResponse(true, rsResponse.Message);
                }
                else if (!(bool)rsResponse.Status)
                {
                    erpResponse = new ErpDeleteCustomerPaymentMethodResponse(false, rsResponse.Message);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpTriggerDataSyncResponse TriggerDataSync(string requestXML)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ErpTriggerDataSyncResponse(false, string.Empty, string.Empty, string.Empty);
            try
            {
                var rsResponse = ECL_TV_TriggerDataSync(requestXML);

                erpResponse = new ErpTriggerDataSyncResponse((bool)rsResponse.Status, rsResponse.Code, rsResponse.Message, rsResponse.Email);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception ex)
            {
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpCustomerBankAccountResponse CreateCustomerBankAccount(ErpCreateCardRequest erpCreateCardRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCustomerBankAccountResponse erpResponse = new ErpCustomerBankAccountResponse(false, null, null);
            try
            {
                //var rsResponse = ECL_CreateCustomerBankAccount(erpCreateCardRequest);
                //if (rsResponse.Success)
                //{
                //    erpResponse = new ErpCustomerBankAccountResponse(true, rsResponse.Message, new ErpBankAccount(rsResponse.BankAccountRecId, rsResponse.MandateRecId));
                //}
                //else
                //{
                //    erpResponse = new ErpCustomerBankAccountResponse(false, rsResponse.Message, new ErpBankAccount(rsResponse.BankAccountRecId, rsResponse.MandateRecId));
                //}
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpResponse = new ErpCustomerBankAccountResponse(false, exp.Message, null);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        #region "Private"
        private ErpCustomerInvoiceDetail GetErpCustomerInvoice(CustomerInvoiceDetail custInvoice)
        {
            ErpCustomerInvoiceDetail customerInvoice = new ErpCustomerInvoiceDetail();
            try
            {
                customerInvoice.CustomerInvoiceJour.RecId = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.RecId) == true ? customerInvoice.CustomerInvoiceJour.RecId : long.Parse(custInvoice.CustInvoiceJour.RecId);
                customerInvoice.CustomerInvoiceJour.Email = custInvoice.CustInvoiceJour.Email;
                customerInvoice.CustomerInvoiceJour.InvoiceId = custInvoice.CustInvoiceJour.InvoiceId;
                customerInvoice.CustomerInvoiceJour.SalesId = custInvoice.CustInvoiceJour.SalesId;
                customerInvoice.CustomerInvoiceJour.IsMigratedOrder = custInvoice.CustInvoiceJour.IsMigratedOrder;
                customerInvoice.CustomerInvoiceJour.SalesType = custInvoice.CustInvoiceJour.SalesType;
                customerInvoice.CustomerInvoiceJour.InvoiceDate = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.InvoiceDate) == true ? customerInvoice.CustomerInvoiceJour.InvoiceDate : DateTime.Parse(custInvoice.CustInvoiceJour.InvoiceDate);
                customerInvoice.CustomerInvoiceJour.CurrencyCode = custInvoice.CustInvoiceJour.CurrencyCode;
                customerInvoice.CustomerInvoiceJour.InvoiceAmount = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.InvoiceAmount) == true ? customerInvoice.CustomerInvoiceJour.InvoiceAmount : double.Parse(custInvoice.CustInvoiceJour.InvoiceAmount);
                customerInvoice.CustomerInvoiceJour.MigratedInvoiceAmount = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.MigratedInvoiceAmount) == true ? customerInvoice.CustomerInvoiceJour.MigratedInvoiceAmount : double.Parse(custInvoice.CustInvoiceJour.MigratedInvoiceAmount);
                customerInvoice.CustomerInvoiceJour.SalesBalance = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.SalesBalance) == true ? customerInvoice.CustomerInvoiceJour.SalesBalance : double.Parse(custInvoice.CustInvoiceJour.SalesBalance);
                customerInvoice.CustomerInvoiceJour.MCRDueAmount = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.MCRDueAmount) == true ? customerInvoice.CustomerInvoiceJour.MCRDueAmount : double.Parse(custInvoice.CustInvoiceJour.MCRDueAmount);
                customerInvoice.CustomerInvoiceJour.BalanceAmount = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.BalanceAmount) == true ? customerInvoice.CustomerInvoiceJour.BalanceAmount : double.Parse(custInvoice.CustInvoiceJour.BalanceAmount);
                customerInvoice.CustomerInvoiceJour.SumLineDisc = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.SumLineDisc) == true ? customerInvoice.CustomerInvoiceJour.SumLineDisc : double.Parse(custInvoice.CustInvoiceJour.SumLineDisc);
                customerInvoice.CustomerInvoiceJour.SumTax = string.IsNullOrWhiteSpace(custInvoice.CustInvoiceJour.SumTax) == true ? customerInvoice.CustomerInvoiceJour.SumTax : double.Parse(custInvoice.CustInvoiceJour.SumTax);
                customerInvoice.CustomerInvoiceJour.InvoiceAccount = custInvoice.CustInvoiceJour.InvoiceAccount;
                customerInvoice.CustomerInvoiceJour.InvoicingName = custInvoice.CustInvoiceJour.InvoicingName;
                customerInvoice.CustomerInvoiceJour.RetailChannel = custInvoice.CustInvoiceJour.RetailChannel;
                customerInvoice.CustomerInvoiceJour.RetailChannelId = custInvoice.CustInvoiceJour.RetailChannelId;
                customerInvoice.CustomerInvoiceJour.ThreeLetterISORegionName = custInvoice.CustInvoiceJour.ThreeLetterISORegionName;
                customerInvoice.CustomerInvoiceJour.Language = custInvoice.CustInvoiceJour.Language;
                customerInvoice.CustomerInvoiceJour.LocalTaxId = custInvoice.CustInvoiceJour.LocalTaxId;
                customerInvoice.CustomerInvoiceJour.TMVBoletoURL = custInvoice.CustInvoiceJour.TMVBoletoURL;
                customerInvoice.CustomerInvoiceJour.TMVBoletoPaymStatus = custInvoice.CustInvoiceJour.TMVBoletoPaymStatus;

                if (custInvoice.CustInvoiceTrans != null)
                {
                    foreach (var custTrans in custInvoice.CustInvoiceTrans)
                    {
                        ErpCustomerInvoiceTrans customerInvoiceTrans = new ErpCustomerInvoiceTrans();

                        customerInvoiceTrans.CurrencyCode = custTrans.CurrencyCode;
                        customerInvoiceTrans.DiscAmount = string.IsNullOrWhiteSpace(custTrans.DiscAmount) == true ? customerInvoiceTrans.DiscAmount : double.Parse(custTrans.DiscAmount);
                        customerInvoiceTrans.InventDimId = custTrans.InventDimId;
                        customerInvoiceTrans.ItemId = custTrans.ItemId + ":" + custTrans.RetailVariantId;
                        customerInvoiceTrans.SalesPrice = string.IsNullOrWhiteSpace(custTrans.SalesPrice) == true ? customerInvoiceTrans.SalesPrice : double.Parse(custTrans.SalesPrice);
                        customerInvoiceTrans.SalesUnit = custTrans.SalesUnit;
                        customerInvoiceTrans.LineAmount = string.IsNullOrWhiteSpace(custTrans.LineAmount) == true ? customerInvoiceTrans.LineAmount : double.Parse(custTrans.LineAmount);
                        customerInvoiceTrans.LineDisc = string.IsNullOrWhiteSpace(custTrans.LineDisc) == true ? customerInvoiceTrans.LineDisc : double.Parse(custTrans.LineDisc);
                        customerInvoiceTrans.LineAmountTax = string.IsNullOrWhiteSpace(custTrans.LineAmountTax) == true ? customerInvoiceTrans.LineAmountTax : double.Parse(custTrans.LineAmountTax);
                        customerInvoiceTrans.LineNum = string.IsNullOrWhiteSpace(custTrans.LineNum) == true ? customerInvoiceTrans.LineNum : double.Parse(custTrans.LineNum);
                        customerInvoiceTrans.Name = custTrans.Name;
                        customerInvoiceTrans.Qty = string.IsNullOrWhiteSpace(custTrans.Qty) == true ? customerInvoiceTrans.Qty : double.Parse(custTrans.Qty);
                        customerInvoiceTrans.TaxWriteCode = custTrans.TaxWriteCode;
                        customerInvoiceTrans.TMVProductType = custTrans.TMVProductType;
                        customerInvoiceTrans.RecId = string.IsNullOrWhiteSpace(custTrans.RecId) == true ? customerInvoiceTrans.RecId : long.Parse(custTrans.RecId);
                        customerInvoiceTrans.SalesLineRecId = string.IsNullOrWhiteSpace(custTrans.SalesLineRecId) == true ? customerInvoiceTrans.SalesLineRecId : long.Parse(custTrans.SalesLineRecId);
                        customerInvoiceTrans.TMVParent = string.IsNullOrWhiteSpace(custTrans.TMVParent) == true ? customerInvoiceTrans.TMVParent : long.Parse(custTrans.TMVParent);

                        customerInvoice.CustomerInvoiceTrans.Add(customerInvoiceTrans);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

            return customerInvoice;
        }
        private List<ErpCustomerInvoice> MapErpCustomerInvoice(CustomerInvoices lstCustInvoice)
        {
            List<ErpCustomerInvoice> customerInvoices = new List<ErpCustomerInvoice>();
            try
            {
                foreach (var custInvoice in lstCustInvoice.CustomerInvoice)
                {
                    ErpCustomerInvoice customerInvoice = new ErpCustomerInvoice();

                    customerInvoice.RecId = string.IsNullOrWhiteSpace(custInvoice.RecId) == true ? customerInvoice.RecId : long.Parse(custInvoice.RecId);
                    customerInvoice.Email = custInvoice.Email;
                    customerInvoice.InvoiceId = custInvoice.InvoiceId;
                    customerInvoice.Voucher = custInvoice.Voucher;
                    customerInvoice.SalesId = custInvoice.SalesId;
                    customerInvoice.TransactionType = custInvoice.TransactionType;
                    customerInvoice.InvoiceDate = string.IsNullOrWhiteSpace(custInvoice.InvoiceDate) == true ? customerInvoice.InvoiceDate : DateTime.Parse(custInvoice.InvoiceDate);
                    customerInvoice.CurrencyCode = custInvoice.CurrencyCode;
                    customerInvoice.InvoiceAmount = string.IsNullOrWhiteSpace(custInvoice.InvoiceAmount) == true ? customerInvoice.InvoiceAmount : decimal.Parse(custInvoice.InvoiceAmount);
                    customerInvoice.BalanceAmount = string.IsNullOrWhiteSpace(custInvoice.BalanceAmount) == true ? customerInvoice.BalanceAmount : decimal.Parse(custInvoice.BalanceAmount);
                    customerInvoice.InvoiceAccount = custInvoice.InvoiceAccount;
                    customerInvoice.Address = custInvoice.Address;
                    customerInvoice.FileName = custInvoice.FileName;
                    customerInvoice.FileURL = custInvoice.FileURL;

                    customerInvoices.Add(customerInvoice);
                }

            }
            catch (Exception)
            {

                throw;
            }

            return customerInvoices;
        }
        #endregion
        /// <summary>
        /// Adds extension property to customer if state is mandatory for the country.
        /// VSTS: 29622
        /// </summary>
        /// <param name="customer"></param>
        private void AddMandatoryStateExtensionProperty(Customer customer)
        {
            var isStateMandatory = customer.Addresses.Any(a =>
                MandatoryStateCountryManager.IsStateMandatoryForCountry(a.ThreeLetterISORegionName));
            customer.ExtensionProperties.Add(new CommerceProperty()
            {
                Key = "IsStateMandatory",
                Value = new CommercePropertyValue()
                {
                    BooleanValue = isStateMandatory
                }
            });
        }

        #region RetailServer API
        //[Trace]
        //private CustomerBankAccountResponse ECL_CreateCustomerBankAccount(ErpCreateCardRequest erpCreateCardRequest)
        //{
        //    var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
        //    var rsResponse = Task.Run(async () => await customerManager.ECL_CreateCustomerBankAccount(
        //        erpCreateCardRequest.Customer.CustomerNo, erpCreateCardRequest.TenderLine.CardOrAccount,
        //        erpCreateCardRequest.TenderLine.IBAN, erpCreateCardRequest.TenderLine.SwiftCode,
        //        erpCreateCardRequest.TenderLine.BankName, baseCompany)).Result;
        //    return rsResponse;
        //}
        [Trace]
        private TriggerDataSyncResponse ECL_TV_TriggerDataSync(string requestXml)
        {
            var objCustomerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            var rsResponse = Task.Run(async () => await objCustomerManager.ECL_TV_TriggerDataSync(requestXml, baseCompany))
                .Result;
            return rsResponse;
        }
        [Trace]
        private DeleteCustomerPaymentMethodResponse ECL_TV_DeleteCustomerPaymentMethod(
            ErpDeleteCustomerPaymentMethodRequest request)
        {
            var objCustomerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            var rsResponse = Task.Run(async () =>
                await objCustomerManager.ECL_TV_DeleteCustomerPaymentMethod(request.CardRecId, request.BankAccountRecId,
                    baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private Customer ECL_CreateNewCustomer(Customer customer, string externalIdentityId, string externalIdentityIssuer, ObservableCollection<CommerceProperty> extensionProperties)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_CreateNewCustomer(customer, externalIdentityId, externalIdentityIssuer, baseChannelId, base.baseCompany, extensionProperties)).Result;
        }
        [Trace]
        private UpdateCustomerContactPersonResponse ECL_UpdateCustomerContactPerson(string erpCustomerAccountNumber, Customer customer, ContactPerson conPerson, ObservableCollection<CommerceProperty> extensionProperties)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_UpdateCustomerContactPerson(customer, erpCustomerAccountNumber, conPerson, base.baseCompany, extensionProperties)).Result;
        }
        [Trace]
        private Customer ECL_UpdateCustomer(Customer customer, ObservableCollection<CommerceProperty> extensionProperties)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_UpdateCustomer(customer, base.baseCompany, extensionProperties)).Result;
        }
        [Trace]
        private Customer ECL_GetCustomer(string accountNumber)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_GetCustomer(accountNumber)).Result;
        }
        [Trace]
        private PagedResult<Customer> ECL_GetCustomersByLicenses(QueryResultSettings customerQuerySetting, string liceceNumberList)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_GetCustomersByLicenses(liceceNumberList, base.baseCompany, customerQuerySetting)).Result;
        }
        [Trace]
        private GetCustomerInfoByInvoiceIdResponse ECL_TV_GetCustomerInfoByInvoiceId(CustomerByInvoiceRequest request)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_TV_GetCustomerInfoByInvoiceId(request.InvoiceId, baseCompany)).Result;
        }
        [Trace]
        private GetCustomerInvoiceByInvoiceIdResponse ECL_TV_GetCustomerInvoiceByInvoiceId(CustomerInvoiceDetailRequest request)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_TV_GetCustomerInvoiceByInvoiceId(request.InvoiceId, request.CustomerEmail, baseCompany)).Result;
        }
        [Trace]
        private GetCustomerInvoicesResponse ECL_TV_GetCustomerInvoices(CustomerInvoiceRequest request)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_TV_GetCustomerInvoices(request.CustomerAccount, request.LicenseID, request.TransactionType, baseCompany)).Result;
        }
        [Trace]
        private Customer ECL_RTS_GetCustomer(string accountNumber)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_RTS_GetCustomer(accountNumber, baseCompany)).Result;
        }

        [Trace]
        private CreateCustomerPaymentMethodResponse ECL_CreateCustomerPaymentMethod(ErpCreateCardRequest erpCreateCardRequest)
        {
            var creditCardManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            var rsResponse = Task.Run(async () => await creditCardManager.ECL_CreateCustomerPaymentMethod(
                erpCreateCardRequest.Customer.CustomerNo, erpCreateCardRequest.TenderLine.UniqueCardId,
                erpCreateCardRequest.TenderLine.CardToken, erpCreateCardRequest.TenderLine.Authorization, baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private GetCustomerPaymentMethodsResponse ECL_GetCustomerPaymentMethods(ErpGetCardRequest erpGetCardRequest,
            string cardProcessors)
        {
            var creditCardManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await creditCardManager.ECL_GetCustomerPaymentMethods(erpGetCardRequest.customerAccount, 
                                                                 erpGetCardRequest.licenseId, cardProcessors, baseCompany)).Result;
        }
        //[Trace]
        //private GetCreditCardResponse ECL_UpadteCustomerPaymentMethod(ErpEditCardRequest erpEditCardRequest)
        //{
        //    var creditCardManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
        //    var rsResponse = Task.Run(async () => await creditCardManager.ECL_UpadteCustomerPaymentMethod(
        //        erpEditCardRequest.Customer.CustomerNo, erpEditCardRequest.CardNumber,
        //        erpEditCardRequest.TenderLine.ExpMonth.ToString(), erpEditCardRequest.TenderLine.ExpYear.ToString(),
        //        erpEditCardRequest.TenderLine.CardToken, erpEditCardRequest.TenderLine.Authorization, baseCompany)).Result;
        //    return rsResponse;
        //}
        #endregion

    }
}
